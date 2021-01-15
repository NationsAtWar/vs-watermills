using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace watermills.src {

    public class BlockWaterWheelSegment : Block {

        public override void OnLoaded(ICoreAPI api) {

            base.OnLoaded(api);
        }

        public override float OnGettingBroken(IPlayer player, BlockSelection blockSel, ItemSlot itemslot, float remainingResistance, float dt, int counter) {

            IWorldAccessor world = player.Entity.World;

            if (world == null)
                world = api.World;

            BEWaterWheelSegment beSegment = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BEWaterWheelSegment;
            BlockPos centerPos = beSegment.WaterWheelHubPos;

            Block centerBlock = world.BlockAccessor.GetBlock(centerPos);
            BlockSelection bs = blockSel.Clone();
            bs.Position = centerPos;

            return centerBlock.OnGettingBroken(player, bs, itemslot, remainingResistance, dt, counter);
        }

        public override Cuboidf GetParticleBreakBox(IBlockAccessor blockAccess, BlockPos pos, BlockFacing facing) {

            // being broken by player: break the center block instead
            BEWaterWheelSegment beSegment = blockAccess.GetBlockEntity(pos) as BEWaterWheelSegment;
            BlockPos centerPos = beSegment.WaterWheelHubPos;

            Block centerBlock = blockAccess.GetBlock(centerPos);
            return centerBlock.GetParticleBreakBox(blockAccess, centerPos, facing);
        }

        //Need to override because this fake block has no texture of its own (no texture gives black breaking particles)
        public override int GetRandomColor(ICoreClientAPI capi, BlockPos pos, BlockFacing facing) {

            IBlockAccessor blockAccess = capi.World.BlockAccessor;

            BEWaterWheelSegment beSegment = blockAccess.GetBlockEntity(pos) as BEWaterWheelSegment;
            BlockPos centerPos = beSegment.WaterWheelHubPos;

            Block centerBlock = blockAccess.GetBlock(centerPos);
            return centerBlock.GetRandomColor(capi, centerPos, facing);
        }

        public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f) {

            BEWaterWheelSegment beSegment = world.BlockAccessor.GetBlockEntity(pos) as BEWaterWheelSegment;
            BlockPos centerPos = beSegment.WaterWheelHubPos;

            Block centerBlock = world.BlockAccessor.GetBlock(centerPos);
            centerBlock.OnBlockBroken(world, centerPos, byPlayer, dropQuantityMultiplier);
            return;
        }
    }
}