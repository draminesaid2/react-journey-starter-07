namespace Crm.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using Crm.Extensions;
	using Crm.Helpers;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Extensions.IIdentity;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.MobileViewEngine;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Model.Site;
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Validation;
	using Crm.Model;
	using Crm.Model.Extensions;
	using Crm.Model.Lookups;
	using Crm.Services.Interfaces;
	using Crm.ViewModels;

	using log4net;

	using Microsoft.AspNetCore.Authentication;
	using Microsoft.AspNetCore.Authentication.Cookies;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	using IAuthenticationService = Library.Services.Interfaces.IAuthenticationService;
	using IRedirectProvider = Crm.Services.Interfaces.IRedirectProvider;
	using Task = System.Threading.Tasks.Task;

	[Authorize]
	public class AccountController : Controller
	{
		private readonly IAuthenticationService authenticationService;
		private readonly IUserService userService;
		private readonly ILog logger;
		private readonly IBrowserCapabilities browserCapabilities;
		private readonly IEnumerable<IRedirectProvider> redirectProviders;
		private readonly Site site;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly IEnumerable<ILoginValidator> loginValidators;
		private readonly Func<User> userFactory;
		private readonly Func<Email> emailFactory;
		private readonly Func<Message> messageFactory;
		private readonly IRepositoryWithTypedId<Message, Guid> messageRepository;
		private readonly IRuleValidationService ruleValidationService;
		private readonly IResourceManager resourceManager;
		private readonly IRepositoryWithTypedId<Device,Guid> deviceRepository;
		private readonly IPluginProvider pluginProvider;

		public virtual ActionResult AccessDenied()
		{
			return RedirectToAction("Forbidden", "Error");
		}

		[RenderAction("AccountInfoEdit", Priority = 70)]
		public virtual ActionResult AccountInfoEditEmail()
		{
			return PartialView();
		}

		[RenderAction("AccountInfoView", Priority = 70)]
		public virtual ActionResult AccountInfoViewEmail()
		{
			return PartialView();
		}

		[RenderAction("AccountInfoEdit", Priority = 50)]
		public virtual ActionResult AccountInfoEditLanguage()
		{
			return PartialView();
		}

		[RenderAction("AccountInfoView", Priority = 50)]
		public virtual ActionResult AccountInfoViewLanguage()
		{
			return PartialView();
		}

		[RenderAction("AccountInfoEdit", Priority = 100)]
		public virtual ActionResult AccountInfoEditName()
		{
			return PartialView();
		}

		[RenderAction("AccountInfoView", Priority = 100)]
		public virtual ActionResult AccountInfoViewName()
		{
			return PartialView();
		}

		[RenderAction("AccountInfoEdit", Priority = 60)]
		public virtual ActionResult AccountInfoEditPassword()
		{
			return PartialView();
		}
		
		[RenderAction("AccountInfoView", Priority = 60)]
		public virtual ActionResult AccountInfoViewPassword()
		{
			return PartialView();
		}

		// Methods
		[AllowAnonymous]
		[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
		public virtual ActionResult Login()
		{
			if (!userService.GetActiveUsersQuery().Any())
			{
				return RedirectToAction("Register");
			}

			var viewModel = GetLoginViewModel();

			if (TempData["NewPasswordSent"] != null && (bool)TempData["NewPasswordSent"])
			{
				viewModel.NewPasswordSent = true;
			}

			if (Request.Query["noTenantAvailable"] == "true")
			{
				viewModel.NoTenantAvailable = true;
			}

			return View("Login", viewModel);
		}

		[AllowAnonymous]
		[HttpPost]
		public virtual ActionResult Login(string email, string password, bool? rememberMe, string returnUrl, bool? needRedirect)
		{
			User user = null;

			rememberMe = rememberMe ?? false;
			email = email.Trim();

			var ruleViolations = new List<RuleViolation>();

			if (password.IsNullOrEmpty() || !authenticationService.Validate(email, password, out user))
			{
				ruleViolations.Add(new RuleViolation("InvalidLoginData"));
			}
			ruleViolations.AddRange(loginValidators.SelectMany(x => x.GetRuleViolations(user)));

			if (ruleViolations.Any())
			{
				return RedirectToLogin(ruleViolations, rememberMe);
			}

			var cookieOptions = new CookieOptions
			{
				Expires = (bool)rememberMe ? DateTime.Now.AddYears(50) : DateTime.Now.AddDays(-1),
				Path = "/"
			};
			Response.Cookies.Append("Login", "pre-populated", cookieOptions);
			Debug.Assert(user != null, "user != null");

			authenticationService.SignIn(user, (bool)rememberMe);

			var loginMessage = new StringBuilder();			
			loginMessage.AppendFormatLine("User {0} successfully logged in.", user.Id.HideSensitiveDataFromName());
			loginMessage.AppendLine(String.Empty);
			loginMessage.AppendLine(GetUserData());
			logger.InfoFormat(loginMessage.ToString());

			userService.UpdateLastLogin(user);

			var availableClients = redirectProviders.Select(x => x.AvailableClients(user)).Where(x => x != null).Distinct().ToList();
			if (availableClients.Count == 0)
			{
				logger.Error($"User {user.Id.HideSensitiveDataFromName()} has no access to any client. Please contact your system administrator.");
				return new RedirectResult("~/Home/ClientSelection");
			}

			if (needRedirect.HasValue && needRedirect.Value)
			{
				if (availableClients.Count > 1)
				{
					return new RedirectResult("~/Home/ClientSelection");
				}

				switch (availableClients.ElementAt(0))
				{
					case PermissionName.Backend:
						return new RedirectResult("~/Dashboard/Index");
					case PermissionName.MaterialClientOnline:
						return new RedirectResult("~/Home/MaterialIndex");
					case PermissionName.MaterialClientOffline:
						return new RedirectResult("~/Home/MaterialIndex");
					case "VideoClient":
						return new RedirectResult("~/Main.VideoCall/VideoCall/Index");
					default:
						return new RedirectResult("~/Home/ClientSelection");
				}
			}

			if (!String.IsNullOrEmpty(returnUrl))
			{
				if (returnUrl == HttpContext.Request.PathBase && !returnUrl.EndsWith("/"))
				{
					returnUrl += '/';
				}
				if (!returnUrl.StartsWith("/"))
				{
					returnUrl = "/";
				}
				return new RedirectResult(returnUrl);
			}

			var redirects = redirectProviders.Select(x => x.RedirectAfterLogin(user, browserCapabilities, returnUrl)).Where(x => x != null).Distinct().ToArray();
			if (!redirects.Any())
			{
				return new RedirectToRouteResult("Dashboard", null);
			}
			if (redirects.Length == 1)
			{
				return redirects.First();
			}
			return new RedirectResult("~/Home/ClientSelection");
		}

		public virtual ActionResult RedirectToLogin(IList<RuleViolation> ruleViolations, bool? rememberMe)
		{
			var model = GetLoginViewModel();
			model.AddRuleViolations(ruleViolations);
			ViewData["rememberMe"] = rememberMe;
			return View("Login", model);
		}

		[AllowAnonymous]
		public virtual async Task OpenIdLogin(string returnUrl)
		{
			var authProperties = new AuthenticationProperties();
			if (returnUrl != null)
			{
				authProperties.RedirectUri = returnUrl;
			} else
			{
				authProperties.RedirectUri = Url.Home();
			}
			await HttpContext.ChallengeAsync("Auth0", authProperties);
		}

		[AllowAnonymous]
		public virtual RedirectResult OpenIdCallback()
		{
			return new RedirectResult("~/");
		}

		[AllowAnonymous]
		public virtual async Task<ActionResult> Logout()
		{
			if (userService.CurrentUser == null)
				return new EmptyResult();

			var fingerprint = HttpContext.Request.Cookies["fingerprint"];
			if (fingerprint.IsNotNullOrWhiteSpace())
			{
				var device = deviceRepository.GetAll()
					.Where(x => x.Username == userService.CurrentUser.Id)
					.Where(x => x.Fingerprint == HttpContext.Request.Cookies["fingerprint"])
					.FirstOrDefault();

				if (device != null)
				{
					device.Token = null;
					deviceRepository.SaveOrUpdate(device);
				}
			}

			authenticationService.SignOut();

			if (appSettingsProvider.GetValue(MainPlugin.Settings.System.OpenIdAuthentication.UseOpenIdAuthentication))
			{
				await HttpContext.SignOutAsync("Auth0");
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return new EmptyResult();
			}

			return new RedirectResult("~/Account/Login" + Request.QueryString);
		}

		[AllowAnonymous]
		[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
		public virtual ActionResult Register()
		{
			if (userService.GetActiveUsersQuery().Any())
			{
				return RedirectToAction("Login");
			}
			
			var user = userFactory();
			user.DefaultLanguageKey = site.GetExtension<DomainExtension>().DefaultLanguageKey;
			var model = new UserEditViewModel
			{
				Item = user,
				PasswordResetSupported = authenticationService.PasswordResetSupported,
				MinPasswordLength = authenticationService.MinPasswordLength
			};

			return View("Register", model);
		}

		[AllowAnonymous]
		[HttpPost]
		[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
		public virtual ActionResult Register(User user, string confirmPassword)
		{
			if (userService.GetActiveUsersQuery().Any())
			{
				return RedirectToAction("Login");
			}
			
			if (user != null && user.Email.IsNotNullOrEmpty())
			{
				user.Email = user.Email.Trim();
			}

			var ruleViolations = new List<RuleViolation>();
			ruleViolations.AddRange(ruleValidationService.GetRuleViolations(user));

			try
			{
				if (!ruleViolations.Any())
				{
					authenticationService.ResetPassword(user, confirmPassword);
					userService.SaveUser(user);
				}
			}
			catch (Exception ex)
			{
				ruleViolations.Add(new RuleViolation(ex.Message).SetDisplayRegion("UnknownError"));
			}

			var model = new UserEditViewModel
			{
				Item = user,
				MinPasswordLength = authenticationService.MinPasswordLength,
				PasswordResetSupported = authenticationService.PasswordResetSupported
			};

			if (ruleViolations.Any())
			{
				ruleViolations.Where(x => x.DisplayRegion.IsNullOrEmpty()).ForEach(v => v.DisplayRegion = v.PropertyName);
				model.AddRuleViolations(ruleViolations);
				return View("Register", model);
			}

			authenticationService.SignIn(user, false);
			userService.UpdateLastLogin(user);

			return RedirectToAction("Index", "Home");
		}

		[RequiredPermission(PermissionName.Settings, Group = MainPlugin.PermissionGroup.UserAccount)]
		public virtual ActionResult ResetGeneralToken()
		{
			var user = userService.CurrentUser;
			if (user != null)
			{
				var newToken = userService.ResetGeneralToken(user.Email);
				return Json(newToken);
			}
			return Json(new { errorMessage = resourceManager.GetTranslation("UserDoesNotExist") });
		}
		[RequiredPermission(PermissionName.Settings, Group = MainPlugin.PermissionGroup.UserAccount)]
		public virtual ActionResult ResetDropboxToken()
		{
			var user = userService.CurrentUser;
			if (user != null)
			{
				var newToken = userService.ResetDropboxToken(user.Email);
				return Json(newToken);
			}
			return Json(new { errorMessage = resourceManager.GetTranslation("UserDoesNotExist") });
		}

		[AllowAnonymous]
		[HttpGet]
		[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
		public virtual ActionResult ResetPassword()
		{
			return View("ResetPassword", new CrmModel());
		}

		public virtual ActionResult RedirectToResetPassword(IList<RuleViolation> ruleViolations)
		{
			var model = new CrmModel();
			model.AddRuleViolations(ruleViolations);
			return View("ResetPassword", model);
		}

		[AllowAnonymous]
		[HttpPost]
		[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
		public virtual ActionResult ResetPassword(string email)
		{
			var ruleViolations = new List<RuleViolation>();
			var fakeEmail = emailFactory();
			fakeEmail.ContactId = Guid.NewGuid();
			fakeEmail.Data = email;
			fakeEmail.TypeKey = EmailType.WorkKey;
			ruleViolations.AddRange(ruleValidationService.GetRuleViolations(fakeEmail));

			if (ruleViolations.Any())
			{
				return RedirectToResetPassword(ruleViolations);
			}
			
			//Stopwatch to prevent timing attacks
			var stopWatch=new Stopwatch();
			stopWatch.Start();

			var user = userService.GetUsers().GetByEmail(email);
			if (user != null)
			{
				var newPassword = PasswordService.Generate(8);
				var message = messageFactory();
				message.Recipients.Add(email);
				message.Subject = "Your new password";
				message.Body = newPassword;
				messageRepository.SaveOrUpdate(message);
				authenticationService.ResetPassword(user, newPassword);
			}

			while (stopWatch.ElapsedMilliseconds < 500)
				Thread.Sleep(10);

			TempData["NewPasswordSent"] = true;

			var returnUrl = Request.ExtractBackUrlForCurrentUrl();
			if (returnUrl.IsNullOrEmpty())
			{
				return RedirectToAction("Login");
			}
			return Redirect(returnUrl);
		}

		[RequiredPermission(PermissionName.Settings, Group = MainPlugin.PermissionGroup.UserAccount)]
		[HttpPost]
		public virtual ActionResult ChangePassword(string email, string oldPassword, string newPassword, string newPassword2)
		{
			if (oldPassword.IsNullOrEmpty() || !authenticationService.Validate(userService.CurrentUser, oldPassword))
			{
				return new JsonResult(new { ErrorMessageKey = "OldPasswordIsIncorrect" });
			}
			if (!newPassword.IsNullOrEmpty() && !newPassword.Equals(newPassword2, StringComparison.Ordinal))
			{
				return new JsonResult(new { ErrorMessageKey = "PasswordsDoNotMatch" });
			}
			var user = userService.CurrentUser;
			user.Email = email;
			authenticationService.ResetPassword(user, newPassword.IsNullOrEmpty() ? oldPassword : newPassword);
			userService.SaveUser(user);
			return new EmptyResult();
		}

		[RequiredPermission(MainPlugin.PermissionName.ExportDropboxAddressAsVCard, Group = MainPlugin.PermissionGroup.UserAccount)]
		public virtual ActionResult ExportDropboxAddressAsVCard()
		{
			var dropboxDomain = appSettingsProvider.GetValue(MainPlugin.Settings.Dropbox.DropboxDomain);
			var vCard = new VCard { LastName = "CRM Dropbox", Email = userService.CurrentUser.GetDropboxAddress(dropboxDomain) };
			return File(vCard.ToString().GetBytes(), "text/vcf", "CRM Dropbox.vcf");
		}

		[RequiredPermission(PermissionName.Settings, Group = MainPlugin.PermissionGroup.UserAccount)]
		public virtual JsonResult Autocomplete(string q, int limit)
		{
			var users = userService.GetUsers().Where(x => x.DisplayName.Contains(q, StringComparison.OrdinalIgnoreCase)).Take(limit).ToList();

			return Json(users);
		}

		[RequiredPermission(MainPlugin.PermissionName.UpdateStatus, Group = MainPlugin.PermissionGroup.UserAccount)]
		[HttpPost]
		public virtual ActionResult UpdateStatus(User user)
		{
			userService.UpdateStatus(user);
			return RedirectToRoute("Dashboard", new { pageNumber = "Page1" });
		}

		protected virtual string GetUserData()
		{
			if (HttpContext == null)
			{
				return String.Empty;
			}

			var builder = new StringBuilder();
			builder.AppendLine("Request Data");
			var user = HttpContext.User?.Identity.GetUser(userService);
			if (user != null)
			{
				builder.AppendLine("User: " + user.Id.HideSensitiveDataFromName());
			}
			builder.AppendLine("Culture: " + Thread.CurrentThread.CurrentCulture.Name);
			builder.AppendLine("UI Culture: " + Thread.CurrentThread.CurrentUICulture.Name);
			builder.AppendLine("Requested Url: " + HttpContext.Request.Path);
			builder.AppendLine("Referrer Url: " + HttpContext.Request.GetTypedHeaders().Referer);
			builder.AppendLine("Http Method: " + HttpContext.Request.Method);
			builder.AppendLine("Application Version: " + Assembly.GetAssembly(typeof(AccountController)).GetName().Version);
			builder.AppendLine();

			if (HttpContext.Request.Query.Count > 0)
			{
				builder.AppendLine("Query string");
				builder.AppendLine(HttpContext.Request.QueryString.Value);
			}

			builder.AppendLine("Browser Capabilities");
			builder.AppendLine("IP Address: " + HttpContext.Connection.RemoteIpAddress);
			builder.AppendLine("User-Agent: " + HttpContext.Request.UserAgent());
			builder.AppendLine("Identified as mobile device: " + browserCapabilities.IsMobileDevice);
			return builder.ToString().HideSensitiveDataFromUrl();
		}

		protected virtual string GetCordovaDeepLink()
		{
			var hostUri = site.GetExtension<DomainExtension>().HostUri;
			var scheme = hostUri.Scheme.Equals("http") ? "lmobile" : "lmobiles";
			var path = hostUri.PathAndQuery.AppendIfMissing("/") + "Home/Index";
			var uriBuilder = hostUri.IsDefaultPort ? new UriBuilder(scheme, hostUri.Host) { Path = path } : new UriBuilder(scheme, hostUri.Host, hostUri.Port, path);
			return uriBuilder.ToString();
		}

		protected virtual LoginViewModel GetLoginViewModel()
		{
			string avatar = null;
			string userDisplayName = null;
			var loginCookie = String.IsNullOrWhiteSpace(Request?.Cookies["Login"]) ? null : Request.Cookies["Login"];

			var viewModel = new LoginViewModel
			{
				Avatar = avatar,
				CanResetPassword = authenticationService.PasswordResetSupported,
				CordovaDeepLink = GetCordovaDeepLink(),
				LoginCookie = loginCookie,
				LoginType = authenticationService.LoginTypeKey,
				UserDisplayName = userDisplayName,
				UseOpenIdAuthentication = appSettingsProvider.GetValue(MainPlugin.Settings.System.OpenIdAuthentication.UseOpenIdAuthentication)
			};

			if (Request != null && Request.UserAgent() != null)
			{
				if (HttpContext.IsCordovaApp())
				{
					viewModel.IsInCordovaApp = true;
				}
				if (Request.UserAgent().Contains("windows phone 10.", StringComparison.InvariantCultureIgnoreCase) || Request.UserAgent().Contains("windows nt 10.", StringComparison.InvariantCultureIgnoreCase))
				{
					viewModel.IsWindows10Device = true;
				}
				else if (Request.UserAgent().Contains("apple", StringComparison.InvariantCultureIgnoreCase) && (Request.UserAgent().Contains("iPhone", StringComparison.InvariantCultureIgnoreCase) || Request.UserAgent().Contains("iPad", StringComparison.InvariantCultureIgnoreCase) || Request.UserAgent().Contains("iPod", StringComparison.InvariantCultureIgnoreCase)))
				{
					viewModel.IsAppleIosDevice = true;
				}
				else if (Request.UserAgent().Contains("android", StringComparison.InvariantCultureIgnoreCase))
				{
					viewModel.IsAndroidDevice = true;
				}
			}

			viewModel.AppInstallUrlDescriptors = ComposeAppInstallUrlDescriptors(viewModel);

			return viewModel;
		}

		public virtual ActionResult UserProfile()
		{
			return PartialView();
		} 
		[RenderAction("UserProfileTab", Priority = 50)]
		[RequiredPermission(nameof(Device), Group = PermissionGroup.WebApi)]
		public virtual ActionResult UserProfileDevicesTab()
		{ 
			if (!appSettingsProvider.GetValue(MainPlugin.Settings.PushNotification.Enabled))
			{
				return new EmptyResult();
			}

			return PartialView();
		}
		[RenderAction("UserProfileTabHeader", Priority = 50)]
		[RequiredPermission(nameof(Device), Group = PermissionGroup.WebApi)]
		public virtual ActionResult UserProfileDevicesTabHeader()
		{
			if (!appSettingsProvider.GetValue(MainPlugin.Settings.PushNotification.Enabled))
			{
				return new EmptyResult();
			}

			return PartialView();
		}
		[RenderAction("UserProfileTab", Priority = 90)]
		public virtual ActionResult UserProfileAccountInfoTab()
		{
			return PartialView();
		}
		[RenderAction("UserProfileTabHeader", Priority = 90)]
		public virtual ActionResult UserProfileAccountInfoTabHeader()
		{
			return PartialView();
		}
		public virtual ActionResult UserAvatar()
		{
			return PartialView();
		}
		[RenderAction("UserProfileSidebar", Priority = 200)]
		public virtual ActionResult UserProfileName()
		{
			return PartialView();
		}
		[RenderAction("UserProfileTab", Priority = 30)]
		public virtual ActionResult UserProfileDropboxTab()
		{
			return PartialView();
		}
		[RenderAction("UserProfileTabHeader", Priority = 30)]
		public virtual ActionResult UserProfileDropboxTabHeader()
		{
			return PartialView();
		}
		[RenderAction("UserProfileTab", Priority = 20)]
		public virtual ActionResult UserProfileTokenTab()
		{
			return PartialView();
		}
		[RenderAction("UserProfileTabHeader", Priority = 20)]
		public virtual ActionResult UserProfileTokenTabHeader()
		{
			return PartialView();
		}

		[HttpGet]
		public virtual ActionResult GetTurnServerInfo()
		{
			const ushort expiry = 8400;
			const string secret = "2ZP12msSnxnvH3z8N1VUb3P6";
			var cipher = new HMACSHA1(Encoding.UTF8.GetBytes(secret));
			var text = (DateTimeOffset.Now.ToUnixTimeSeconds() + expiry).ToString();
			var result = Convert.ToBase64String(cipher.ComputeHash(Encoding.UTF8.GetBytes(text)));
			return new ContentResult{Content = $"{text}:{result}"};
		}
		
		protected virtual string GenerateXmppLoginToken(User user)
		{
			var cipher = new HMACSHA384(Encoding.ASCII.GetBytes(user.UserId.ToString("N")));
			var result = cipher.ComputeHash(Encoding.UTF8.GetBytes(user.GeneralToken));
			return String.Concat(Array.ConvertAll(result, x => x.ToString("x2")));
		}
		
		[HttpGet]
		public virtual ActionResult GetXmppToken() => 
			new ContentResult { Content = GenerateXmppLoginToken(userService.CurrentUser) };

		public class XmppLoginData
		{
			public string username;
			public string token;
		}
		
		[AllowAnonymous]
		[HttpPost]
		public virtual StatusCodeResult ValidateUserXmppToken([FromBody] XmppLoginData data)
		{
			if (data.username.IsNullOrEmpty() || data.token.IsNullOrEmpty())
			{
				return new StatusCodeResult(403);
			}
			
			var user = userService.GetUser(data.username);
			if (user == null)
			{
				return new StatusCodeResult(403);
			}

			return  GenerateXmppLoginToken(user) == data.token ? new StatusCodeResult(200) : new StatusCodeResult(403);
		}

		public virtual List<MobileAppInstallSourceDescriptor> ComposeAppInstallUrlDescriptors(LoginViewModel loginViewModel)
		{
			var result = new List<MobileAppInstallSourceDescriptor>();

			if (pluginProvider.ActivePluginDescriptors.Any(x => x.PluginName == "Crm.Service"))
			{
				string serviceAppInstallUrl = null;
				if (loginViewModel.IsAndroidDevice)
				{
					serviceAppInstallUrl = appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.AndroidServiceAppLink);
				}
				else if (loginViewModel.IsAppleIosDevice)
				{
					serviceAppInstallUrl = appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.AppleIosServiceAppLink);
				}
				else if (loginViewModel.IsWindows10Device)
				{
					serviceAppInstallUrl = appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.Windows10ServiceAppLink);
				}

				if (!string.IsNullOrEmpty(serviceAppInstallUrl))
				{
					result.Add(new MobileAppInstallSourceDescriptor()
					{
						Url = serviceAppInstallUrl,
						Caption = "Service App",
						LogoPath = "~/Content/img/app-logo-service.webp"
					});
				}
			}

			if (pluginProvider.ActivePluginDescriptors.Any(x => x.PluginName == "Crm.Order"))
			{
				string salesAppInstallUrl = null;
				if (loginViewModel.IsAndroidDevice)
				{
					salesAppInstallUrl = appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.AndroidSalesAppLink);
				}
				else if (loginViewModel.IsAppleIosDevice)
				{
					salesAppInstallUrl = appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.AppleIosSalesAppLink);
				}
				else if (loginViewModel.IsWindows10Device)
				{
					salesAppInstallUrl = appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.Windows10SalesAppLink);
				}

				if (!string.IsNullOrEmpty(salesAppInstallUrl))
				{
					result.Add(new MobileAppInstallSourceDescriptor()
					{
						Url = salesAppInstallUrl,
						Caption = "Sales App",
						LogoPath = "~/Content/img/app-logo-sales.webp"
					});
				}
			}

			return result;
		}

		public AccountController(IAuthenticationService authenticationService,
			IUserService userService,
			IBrowserCapabilities browserCapabilities,
			IEnumerable<IRedirectProvider> redirectProviders,
			Site site, IAppSettingsProvider appSettingsProvider,
			IResourceManager resourceManager,
			IRuleValidationService ruleValidationService,
			ILog logger, IEnumerable<ILoginValidator> loginValidators,
			Func<User> userFactory, Func<Email> emailFactory,
			IRepositoryWithTypedId<Message, Guid> messageRepository,
			Func<Message> messageFactory,
			IRepositoryWithTypedId<Device, Guid> deviceRepository,
			IPluginProvider pluginProvider)
		{
			this.authenticationService = authenticationService;
			this.userService = userService;
			this.browserCapabilities = browserCapabilities;
			this.redirectProviders = redirectProviders;
			this.site = site;
			this.appSettingsProvider = appSettingsProvider;
			this.resourceManager = resourceManager;
			this.ruleValidationService = ruleValidationService;
			this.logger = logger;
			this.loginValidators = loginValidators;
			this.userFactory = userFactory;
			this.emailFactory = emailFactory;
			this.messageFactory = messageFactory;
			this.messageRepository = messageRepository;
			this.deviceRepository = deviceRepository;
			this.pluginProvider = pluginProvider;
		}
	}
}
