
<?php

return [
    /*
    |--------------------------------------------------------------------------
    | Cross-Origin Resource Sharing (CORS) Configuration
    |--------------------------------------------------------------------------
    |
    | Ce fichier configure les paramètres de partage de ressources cross-origin
    | ou "CORS". Cela détermine quelles opérations cross-origin peuvent être exécutées
    | dans les navigateurs web. Vous pouvez ajuster ces paramètres selon vos besoins.
    |
    | Pour en savoir plus: https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS
    |
    */

    'paths' => ['api/*', 'sanctum/csrf-cookie'],

    'allowed_methods' => ['*'],

    'allowed_origins' => ['http://localhost:5173', 'http://127.0.0.1:5173'],

    'allowed_origins_patterns' => [],

    'allowed_headers' => ['*'],

    'exposed_headers' => ['*'],

    'max_age' => 7200,

    'supports_credentials' => true,
];
