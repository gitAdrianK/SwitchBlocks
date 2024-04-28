using EntityComponent;
using HarmonyLib;
using JumpKing.API;
using JumpKing.Level;
using JumpKing.Mods;
using JumpKing.Player;
using Microsoft.Xna.Framework;
using SwitchBlocksMod.Behaviours;
using SwitchBlocksMod.Blocks;
using SwitchBlocksMod.Entities;
using SwitchBlocksMod.Factories;
using SwitchBlocksMod.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SwitchBlocksMod
{
    [JumpKingMod("Zebra.SwitchBlocksMod")]
    public static class ModEntry
    {
        public static bool IsEven = true; //It checks Jumps is even or odd.(even:true)
        static void jumpswitch() {
            IsEven = !IsEven;
        }
        /// <summary>
        /// Called by Jump King before the level loads
        /// -> OnGameStart
        /// </summary>
        [BeforeLevelLoad]
        public static void BeforeLevelLoad()
        {
            //Debugger.Launch();

            List<(bool, Type)> blockFactories = new List<(bool, Type)>
            {
                (ModBlocks.IS_AUTO_FUNCTIONALLY_INITIALIZED, typeof(FactoryAuto)),
                (ModBlocks.IS_BASIC_FUNCTIONALLY_INITIALIZED, typeof(FactoryBasic)),
                (ModBlocks.IS_COUNTDOWN_FUNCTIONALLY_INITIALIZED, typeof(FactoryCountdown)),
                (ModBlocks.IS_SAND_FUNCTIONALLY_INITIALIZED, typeof(FactorySand)),
            };
            foreach (var item in blockFactories.Where(pair => pair.Item1))
            {
                var constructorInfo = item.Item2.GetConstructor(Type.EmptyTypes);
                LevelManager.RegisterBlockFactory((IBlockFactory)constructorInfo.Invoke(null));
            };
            LevelManager.RegisterBlockFactory(new JumpSwitchBlockONFactory());
            LevelManager.RegisterBlockFactory(new JumpSwitchBlockOFFFactory());
            if (ModBlocks.IS_SAND_FUNCTIONALLY_INITIALIZED)
            {
                var harmony = new Harmony("Zebra.SwitchBlocksMod");
                // harmony.PatchAll(Assembly.GetExecutingAssembly());
                MethodInfo isOnBlockMethod = typeof(BodyComp).GetMethod("IsOnBlock", new Type[] { typeof(Type) });
                MethodInfo postfixMethod = typeof(ModEntry).GetMethod("IsOnBlockPostfix");
                originalIsOnBlock = harmony.Patch(isOnBlockMethod);
                harmony.Patch(isOnBlockMethod, postfix: new HarmonyMethod(postfixMethod));
            }
        }

        /// <summary>
        /// Called by Jump King when the Level Starts
        /// </summary>
        [OnLevelStart]
        public static void OnLevelStart()
        {
            ModSaves.Load();
            ModSounds.Load();
            EntityManager entityManager = EntityManager.instance;
            PlayerEntity player = entityManager.Find<PlayerEntity>();

            if (player == null)
            {
                return;
            }

            // Auto
            if (ModBlocks.IS_AUTO_FUNCTIONALLY_INITIALIZED)
            {
                if (EntityAutoPlatforms.Instance.PlatformDictionary != null)
                {
                    entityManager.AddObject(EntityAutoPlatforms.Instance);
                }
            }
            // Basic
            if (ModBlocks.IS_BASIC_FUNCTIONALLY_INITIALIZED)
            {
                if (EntityBasicPlatforms.Instance.PlatformDictionary != null
                    && EntityBasicLevers.Instance.LeverDictionary != null)
                {
                    entityManager.AddObject(EntityBasicPlatforms.Instance);
                    entityManager.AddObject(EntityBasicLevers.Instance);
                }
                BehaviourBasicLever behaviourBasicLever = new BehaviourBasicLever();
                List<(Color?, Type, IBlockBehaviour)> blocks = new List<(Color?, Type, IBlockBehaviour)>
                {
                    (ModBlocks.BASIC_LEVER, typeof(BlockBasicLever), behaviourBasicLever),
                    (ModBlocks.BASIC_LEVER_ON, typeof(BlockBasicLeverOn), behaviourBasicLever),
                    (ModBlocks.BASIC_LEVER_OFF, typeof(BlockBasicLeverOff), behaviourBasicLever),
                    (ModBlocks.BASIC_LEVER_SOLID, typeof(BlockBasicLeverSolid), behaviourBasicLever),
                    (ModBlocks.BASIC_LEVER_SOLID_ON, typeof(BlockBasicLeverSolidOn), behaviourBasicLever),
                    (ModBlocks.BASIC_LEVER_SOLID_OFF, typeof(BlockBasicLeverSolidOff), behaviourBasicLever),
                };
                foreach (var item in blocks.Where(triple => triple.Item1 != null))
                {
                    player.m_body.RegisterBlockBehaviour(item.Item2, item.Item3);
                }
            }
            // Countdown
            if (ModBlocks.IS_COUNTDOWN_FUNCTIONALLY_INITIALIZED)
            {
                if (EntityCountdownPlatforms.Instance.PlatformDictionary != null
                    && EntityCountdownLevers.Instance.LeverDictionary != null)
                {
                    entityManager.AddObject(EntityCountdownPlatforms.Instance);
                    entityManager.AddObject(EntityCountdownLevers.Instance);
                }
                BehaviourCountdownLever behaviourCountdownLever = new BehaviourCountdownLever();
                List<(Color?, Type, IBlockBehaviour)> blocks = new List<(Color?, Type, IBlockBehaviour)>
                {
                    (ModBlocks.COUNTDOWN_LEVER, typeof(BlockCountdownLever), behaviourCountdownLever),
                    (ModBlocks.COUNTDOWN_LEVER_SOLID, typeof(BlockCountdownLeverSolid), behaviourCountdownLever),
                };
                foreach (var item in blocks.Where(triple => triple.Item1 != null))
                {
                    player.m_body.RegisterBlockBehaviour(item.Item2, item.Item3);
                }
            }
            // Sand
            if (ModBlocks.IS_SAND_FUNCTIONALLY_INITIALIZED)
            {
                if (EntitySandPlatforms.Instance.PlatformDictionary != null
                    && EntityCountdownLevers.Instance.LeverDictionary != null)
                {
                    entityManager.AddObject(EntitySandPlatforms.Instance);
                    entityManager.AddObject(EntitySandLevers.Instance);
                }
                BehaviourSandPlatform behaviourSand = new BehaviourSandPlatform();
                BehaviourSandLever behaviourSandLever = new BehaviourSandLever();
                List<(Color?, Type, IBlockBehaviour)> blocks = new List<(Color?, Type, IBlockBehaviour)>
                {
                    (ModBlocks.SAND_ON, typeof(BlockSandOn), behaviourSand),
                    (ModBlocks.SAND_OFF, typeof(BlockSandOff), behaviourSand),
                    (ModBlocks.SAND_LEVER, typeof(BlockSandLever), behaviourSandLever),
                    (ModBlocks.SAND_LEVER_ON, typeof(BlockSandLeverOn), behaviourSandLever),
                    (ModBlocks.SAND_LEVER_OFF, typeof(BlockSandLeverOff), behaviourSandLever),
                    (ModBlocks.SAND_LEVER_SOLID, typeof(BlockSandLeverSolid), behaviourSandLever),
                    (ModBlocks.SAND_LEVER_SOLID_ON, typeof(BlockSandLeverSolidOn), behaviourSandLever),
                    (ModBlocks.SAND_LEVER_SOLID_OFF, typeof(BlockSandLeverSolidOff), behaviourSandLever),
                };
                foreach (var item in blocks.Where(triple => triple.Item1 != null))
                {
                    player.m_body.RegisterBlockBehaviour(item.Item2, item.Item3);
                }
            }

            // for jumpswitch blocks
            JumpKing.Player.PlayerEntity.OnJumpCall += jumpswitch;
            // End
            EntityManager.instance.MoveToFront(player);
        }

        /// <summary>
        /// Called by Jump King when the Level Ends
        /// </summary>
        [OnLevelEnd]
        public static void OnLevelEnd()
        {
            EntityManager entityManager = EntityManager.instance;
            entityManager.RemoveObject(EntityAutoPlatforms.Instance);
            entityManager.RemoveObject(EntityBasicPlatforms.Instance);
            entityManager.RemoveObject(EntityBasicLevers.Instance);
            entityManager.RemoveObject(EntityCountdownPlatforms.Instance);
            entityManager.RemoveObject(EntityCountdownLevers.Instance);
            entityManager.RemoveObject(EntitySandPlatforms.Instance);
            entityManager.RemoveObject(EntitySandLevers.Instance);
            EntityAutoPlatforms.Instance.Reset();
            EntityBasicPlatforms.Instance.Reset();
            EntityBasicLevers.Instance.Reset();
            EntityCountdownPlatforms.Instance.Reset();
            EntityCountdownLevers.Instance.Reset();
            EntitySandPlatforms.Instance.Reset();
            EntitySandLevers.Instance.Reset();
            ModSounds.Reset();
            ModSaves.Save();
        }

        private static MethodInfo originalIsOnBlock;
        /// <summary>
        /// Function to be patched in with harmony, adds the custom sand blocks from this mod to also return true
        /// in the IsOnBlock function when asked if the player is on a sand block.
        /// </summary>
        /// <param name="__instance">Object instance of the body comp</param>
        /// <param name="__result">Result of the patched function, returning true if the player is on any sand block</param>
        /// <param name="__0">Original object the function is called with</param>
        public static void IsOnBlockPostfix(object __instance, ref bool __result, Type __0)
        {
            if (__0 == typeof(SandBlock) && originalIsOnBlock != null)
            {
                __result = (bool)originalIsOnBlock.Invoke(null, new object[] { (BodyComp)__instance, typeof(SandBlock) })
                    || (bool)originalIsOnBlock.Invoke(null, new object[] { (BodyComp)__instance, typeof(BlockSandOn) })
                    || (bool)originalIsOnBlock.Invoke(null, new object[] { (BodyComp)__instance, typeof(BlockSandOff) });
            }
        }
    }

    public class JumpSwitchBlockON : IBlock
    {
        private readonly Rectangle m_collider;

        public JumpSwitchBlockON(Rectangle p_collider)
        {
            m_collider = p_collider;
        }

        public Rectangle GetRect()
        {
            return ModEntry.IsEven ? m_collider : new Rectangle(0, 0, 0, 0);
        }

        public bool IsSolidBlock(Color blockCode)
        {
            return ModEntry.IsEven;
        }

        public BlockCollisionType Intersects(Rectangle p_hitbox, out Rectangle p_intersection)
        {
            bool ret = m_collider.Intersects(p_hitbox);

            if (ret)
            {
                p_intersection = Rectangle.Intersect(p_hitbox, m_collider);
                return ModEntry.IsEven ? BlockCollisionType.Collision_Blocking : BlockCollisionType.Collision_NonBlocking;
            }
            else
            {
                p_intersection = new Rectangle(0, 0, 0, 0);
                return BlockCollisionType.NoCollision;
            }
        }
    }

    public class JumpSwitchBlockONFactory : IBlockFactory
    {
        private static readonly Color BLOCKCODE_LOWGRAVITY = new Color(95, 95, 95);

        private readonly HashSet<Color> supportedBlockCodes = new HashSet<Color>()
        {
            BLOCKCODE_LOWGRAVITY,
        };

        public bool CanMakeBlock(Color blockCode, Level level)
        {
            return supportedBlockCodes.Contains(blockCode);
        }

        public bool IsSolidBlock(Color blockCode)
        {
            return !ModEntry.IsEven;
        }

        public IBlock GetBlock(Color blockCode, Rectangle blockRect, JumpKing.Workshop.Level level, LevelTexture textureSrc, int currentScreen, int x, int y)
        {
            if (blockCode == BLOCKCODE_LOWGRAVITY)
            {
                return new JumpSwitchBlockON(blockRect);
            }
            else
            {
                throw new InvalidOperationException($"It is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }

    public class JumpSwitchBlockOFF : IBlock
    {
        private readonly Rectangle m_collider;

        public JumpSwitchBlockOFF(Rectangle p_collider)
        {
            m_collider = p_collider;
        }

        public Rectangle GetRect()
        {
            return !ModEntry.IsEven ? m_collider : new Rectangle(0, 0, 0, 0);
        }

        public bool IsSolidBlock(Color blockCode)
        {
            return !ModEntry.IsEven;
        }

        public BlockCollisionType Intersects(Rectangle p_hitbox, out Rectangle p_intersection)
        {
            bool ret = m_collider.Intersects(p_hitbox);

            if (ret)
            {
                p_intersection = Rectangle.Intersect(p_hitbox, m_collider);
                return ModEntry.IsEven ? BlockCollisionType.Collision_NonBlocking : BlockCollisionType.Collision_Blocking;
            }
            else
            {
                p_intersection = new Rectangle(0, 0, 0, 0);
                return BlockCollisionType.NoCollision;
            }
        }
    }

    public class JumpSwitchBlockOFFFactory : IBlockFactory
    {
        private static readonly Color BLOCKCODE_LOWGRAVITY = new Color(31, 31, 31);

        private readonly HashSet<Color> supportedBlockCodes = new HashSet<Color>()
        {
            BLOCKCODE_LOWGRAVITY,
        };

        public bool CanMakeBlock(Color blockCode, Level level)
        {
            return supportedBlockCodes.Contains(blockCode);
        }

        public bool IsSolidBlock(Color blockCode)
        {
            return !ModEntry.IsEven;
        }

        public IBlock GetBlock(Color blockCode, Rectangle blockRect, JumpKing.Workshop.Level level, LevelTexture textureSrc, int currentScreen, int x, int y)
        {
            if (blockCode == BLOCKCODE_LOWGRAVITY)
            {
                return new JumpSwitchBlockOFF(blockRect);
            }
            else
            {
                throw new InvalidOperationException($"It is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
