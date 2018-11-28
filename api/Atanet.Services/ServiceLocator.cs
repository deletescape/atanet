namespace Atanet.Services
{
    using System;

    // Avoid using this class
    public static class ServiceLocator
    {
        private static Func<IServiceProvider> serviceProvider;

        public static IServiceProvider Instance => ServiceLocator.serviceProvider?.Invoke();

        public static void SetServiceLocator(Func<IServiceProvider> serviceLocator)
        {
            ServiceLocator.serviceProvider = serviceLocator;
        }
    }
}
