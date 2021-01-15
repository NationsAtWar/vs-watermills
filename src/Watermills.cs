using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace watermills.src {

    public class Watermills : ModSystem {

        public override void Start(ICoreAPI api) {

            base.Start(api);

            // Register base water wheel block and properties
            api.RegisterBlockClass("blockwaterwheel", typeof(BlockWaterWheel));
            api.RegisterBlockEntityClass("bewaterwheel", typeof(BEWaterWheel));
            api.RegisterBlockEntityBehaviorClass("bebehaviorwaterwheel", typeof(BEBehaviorWaterWheel));

            // Shouldn't use fake blocks until the game allows objects to be underwater and not mess with water flow.
            // There may be a way to edit the base game to allow blocks to extend their collision and selection boxes beyond their origin

            // Register the 'empty' surrounding blocks
            //api.RegisterBlockClass("blockwaterwheelsegment", typeof(BlockWaterWheelSegment));
            //api.RegisterBlockEntityClass("bewaterwheelsegment", typeof(BEWaterWheelSegment));
        }

        public static BlockPos[] GetPaddlePositions(BlockPos hubPosition, bool axisNS) {

            BlockPos[] paddlePositions = new BlockPos[8];

            int i = 0;

            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {

                    // Ignore center position
                    if (x == 0 && y == 0)
                        continue;

                    if (axisNS)
                        paddlePositions[i] = new BlockPos(hubPosition.X + x, hubPosition.Y + y, hubPosition.Z);
                    else
                        paddlePositions[i] = new BlockPos(hubPosition.X, hubPosition.Y + y, hubPosition.Z + x);

                    i++;
                }
            }

            return paddlePositions;
        }
    }
}