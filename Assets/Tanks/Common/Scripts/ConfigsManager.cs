using SO.Scripts;

namespace Tanks.Common.Scripts
{
    public class ConfigsManager
    {
        private readonly ColorsConfig _colorsConfig;
        private readonly TanksConfig _tanksConfig;
        
        public ColorsConfig ColorsConfig => _colorsConfig;
        public TanksConfig TanksConfig => _tanksConfig;

        public ConfigsManager(ColorsConfig colorsConfig, TanksConfig tanksConfig)
        {
            _colorsConfig = colorsConfig;
            _tanksConfig = tanksConfig;
        }
        
        
    }
}