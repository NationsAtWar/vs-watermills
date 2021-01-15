using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace watermills.src {

    public class BEWaterWheel : BlockEntity {

        public bool AxisNS = false;

        public override void Initialize(ICoreAPI api) {

            base.Initialize(api);
        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve) {

            base.FromTreeAttributes(tree, worldAccessForResolve);
        }

        public override void ToTreeAttributes(ITreeAttribute tree) {

            base.ToTreeAttributes(tree);
        }

        public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb) {

            base.GetBlockInfo(forPlayer, sb);
        }
    }
}