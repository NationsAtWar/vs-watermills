using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace watermills.src {

    public class BlockWaterWheel : BlockMPBase {

        private BlockFacing powerOutFacing;

        public override void OnLoaded(ICoreAPI api) {

            powerOutFacing = BlockFacing.FromCode(Variant["side"]).Opposite;
            base.OnLoaded(api);
        }

        public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode) {

            if (CanPlaceBlock(world, byPlayer, blockSel, ref failureCode)) {

                BlockFacing[] horVer = SuggestedHVOrientation(byPlayer, blockSel);
                BlockPos blockPos = blockSel.Position;

                AssetLocation blockCode = CodeWithParts(horVer[0].Code);
                Block block = world.BlockAccessor.GetBlock(blockCode);
                world.BlockAccessor.SetBlock(block.BlockId, blockPos);

                // Stores what axis the waterwheel is facing inside its block entity
                if (world.BlockAccessor.GetBlockEntity(blockPos) is BEWaterWheel beWheel && blockSel.Face.IsAxisNS)
                    beWheel.AxisNS = true;

                //PlaceFakeBlocks(world, blockSel);

                foreach (BlockFacing face in BlockFacing.HORIZONTALS) {

                    BlockPos pos = blockPos.AddCopy(face);

                    if (world.BlockAccessor.GetBlock(pos) is IMechanicalPowerBlock mechblock) {

                        if (mechblock.HasMechPowerConnectorAt(world, pos, face.Opposite)) {

                            mechblock.DidConnectAt(world, pos, face.Opposite);
                            WasPlaced(world, blockPos, face);

                            BEBehaviorMPBase beMechBase = world.BlockAccessor.GetBlockEntity(blockPos).GetBehavior<BEBehaviorMPBase>();
                            WasPlaced(world, blockPos, null);

                            if (beMechBase != null)
                                beMechBase.tryConnect(face);

                            return true;
                        }
                    }
                }

                return true;
            } else
                return false;
        }

        public override bool HasMechPowerConnectorAt(IWorldAccessor world, BlockPos pos, BlockFacing face) {

            return (face == powerOutFacing || face == powerOutFacing.Opposite);
        }

        public override bool CanPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref string failureCode) {

            if (!base.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
                return false;

            BlockPos pos = blockSel.Position;

            // All surrounding paddle blocks must be placeable in order for the whole wheel to be placed
            foreach (BlockPos paddlePos in Watermills.GetPaddlePositions(pos, blockSel.Face.IsAxisNS)) {

                BlockSelection paddleSelection = blockSel.Clone();
                paddleSelection.Position = paddlePos;

                if (!base.CanPlaceBlock(world, byPlayer, paddleSelection, ref failureCode))
                    return false;
            }

            return true;
        }

        public override void OnBlockRemoved(IWorldAccessor world, BlockPos pos) {

            base.OnBlockRemoved(world, pos);
        }

        public override AssetLocation GetHorizontallyFlippedBlockCode(EnumAxis axis) {

            BlockFacing facing = BlockFacing.FromCode(LastCodePart());

            if (facing.Axis == axis)
                return CodeWithParts(facing.Opposite.Code);

            return Code;
        }

        public override void DidConnectAt(IWorldAccessor world, BlockPos pos, BlockFacing face) {

            return;
        }
    }
}