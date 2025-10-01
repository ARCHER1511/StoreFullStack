using Domain.Interfaces;

namespace Domain.Common
{
    public static class CodeGenerator
    {
        private static int _counter;
        private static bool _initialized;

        public static async Task InitializeAsync(IProductCodeProvider provider)
        {
            if (_initialized)
                return;

            var current = await provider.GetCurrentMaxNumberAsync();
            Interlocked.Exchange(ref _counter, current);
            _initialized = true;
        }

        public static async Task<string> GenerateNextAvailableCodeAsync(IProductCodeProvider provider)
        {
            int next = (await provider.GetNextAvailableNumberAsync()) ?? 1;
            return $"P{next:00}";
        }
    }
}
