import { Zap, Shield, Smartphone } from "lucide-react";

const Features = () => {
  const features = [
    {
      icon: <Zap className="w-8 h-8 text-blue-600" />,
      title: "Lightning Fast",
      description: "Built with performance in mind for the best user experience"
    },
    {
      icon: <Shield className="w-8 h-8 text-blue-600" />,
      title: "Secure by Default",
      description: "Enterprise-grade security built into every feature"
    },
    {
      icon: <Smartphone className="w-8 h-8 text-blue-600" />,
      title: "Mobile First",
      description: "Responsive design that works perfectly on all devices"
    }
  ];

  return (
    <section id="features" className="py-20 bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <h2 className="text-3xl md:text-4xl font-bold text-center mb-12">
          Amazing Features
        </h2>
        <div className="grid md:grid-cols-3 gap-8">
          {features.map((feature, index) => (
            <div
              key={index}
              className="p-6 bg-white rounded-lg shadow-md hover:shadow-lg transition-shadow"
            >
              <div className="mb-4">{feature.icon}</div>
              <h3 className="text-xl font-semibold mb-2">{feature.title}</h3>
              <p className="text-gray-600">{feature.description}</p>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
};

export default Features;