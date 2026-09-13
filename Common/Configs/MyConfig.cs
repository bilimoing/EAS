using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace EAS.Common.Configs
{
    public class MyConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        [Header("Display")]
        [DefaultValue(true)]
        public bool Balloon;
        [DefaultValue(true)]
        public bool Shield;
        [DefaultValue(true)]
        public bool Shoe;
        [DefaultValue(true)]
        public bool Waist;
        [DefaultValue(true)]
        public bool Wing;
        [DefaultValue(true)]
        public bool Beard;
        [DefaultValue(true)]
        public bool Neck;
        [DefaultValue(true)]
        public bool Back;
        [DefaultValue(true)]
        public bool Face;       
        [DefaultValue(true)]
        public bool LifeRegen;
        [DefaultValue(true)]
        public bool Expert;
        [DefaultValue(true)]
        public bool Master;
        [DefaultValue(true)]
        public bool MusicBox;
        [DefaultValue(true)]
        public bool Glove;
    }
}
