namespace UIParticle.Service
{
    public static class UIParticlesGlobal
    {
        public static IUIParticleService Instance { get; internal set; }
        public static bool Initialized { get; internal set; }

        public static void Initialize(IUIParticleService uiParticleService)
        {
            Instance = uiParticleService;
            Initialized = false;
        }

        public static void Dispose()
        {
            Initialized = false;
            Instance = null;
        }
    }
}