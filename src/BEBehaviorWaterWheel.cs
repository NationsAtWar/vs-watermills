using System.Text;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace watermills.src {

    public class BEBehaviorWaterWheel : BEBehaviorMPRotor {

        private double waterPower = 0;

        //private AssetLocation sound;
        //protected override AssetLocation Sound => sound;
        //protected override float GetSoundVolume() => ((float) waterPower);

        protected override float Resistance { get { return 0.03f; } }
        protected override double AccelerationFactor { get { return 0.1d; } }
        protected override float TargetSpeed { get { return (float) waterPower; } }
        protected override float TorqueFactor { get { return 5.0f / 4f; } }

        public BEBehaviorWaterWheel(BlockEntity blockentity) : base(blockentity) {

        }

        public override void Initialize(ICoreAPI api, JsonObject properties) {

            base.Initialize(api, properties);

            //sound = new AssetLocation("game:sounds/effect/swoosh");
            Blockentity.RegisterGameTickListener(CheckWaterSpeed, 1000);
        }

        private void CheckWaterSpeed(float dt) {

            if (Api.Side.IsClient())
                return;

            waterPower = 0;

            // Checks the waterflow of each paddle block and figures out which direction and how much power to apply
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {

                    // Ignore center position
                    if (x == 0 && y == 0)
                        continue;

                    BlockPos paddlePosition = new BlockPos(Position.X, Position.Y + y, Position.Z);

                    // Apply x offset depending on waterwheel's y rotation
                    if (((BEWaterWheel)Blockentity).AxisNS)
                        paddlePosition.X += x;
                    else
                        paddlePosition.Z += x;

                    Block wheelBlock = Api.World.BlockAccessor.GetBlock(paddlePosition);
                    string flowDirection = wheelBlock.Variant["flow"];

                    if (flowDirection == null)
                        continue;

                    // Detect North and Western flow
                    if (flowDirection.Equals("n") || flowDirection.Equals("w") || flowDirection.Equals("nw")) {

                        if (y == 1)
                            waterPower -= 0.1d;
                        if (y == -1)
                            waterPower += 0.1d;
                    }

                    // Detect South and Eastern flow
                    if (flowDirection.Equals("s") || flowDirection.Equals("e") || flowDirection.Equals("se")) {

                        if (y == 1)
                            waterPower += 0.1d;
                        if (y == -1)
                            waterPower -= 0.1d;
                    }

                    // Detect Downward flow
                    if (flowDirection.Equals("d")) {

                        if (x == 1)
                            waterPower += 0.1d;
                        if (x == -1)
                            waterPower -= 0.1d;
                    }
                }
            }

            // Determine which direction to apply force to
            if (((BEWaterWheel)Blockentity).AxisNS) {

                if (waterPower < 0) {

                    propagationDir = BlockFacing.SOUTH;
                    waterPower *= -1;
                } else
                    propagationDir = BlockFacing.NORTH;
            } else {
                
                if (waterPower < 0) {

                    propagationDir = BlockFacing.WEST;
                    waterPower *= -1;
                } else
                    propagationDir = BlockFacing.EAST;
            }

            network.updateNetwork(manager.getTickNumber());
        }

        public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb) {

            base.GetBlockInfo(forPlayer, sb);

            sb.AppendLine(string.Format(Lang.Get("Water power: {0}%", (int)(100))));
            sb.AppendLine(Lang.Get("Sails power output: {0} kN", (int)(5 / 5f * 100f)));
        }
    }
}