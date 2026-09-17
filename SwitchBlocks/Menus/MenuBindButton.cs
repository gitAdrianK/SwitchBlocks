namespace SwitchBlocks.Menus
{
    using BehaviorTree;
    using Controls;
    using EntityComponent;
    using EntityComponent.BT;
    using JumpKing.Controller;

    public class MenuBindButton : EntityBTNode
    {
        private readonly EBinding button;
        private readonly int orderIndex;

        public MenuBindButton(Entity entity, EBinding button, int orderIndex) : base(entity)
        {
            this.button = button;
            this.orderIndex = orderIndex;
        }

        protected override BTresult MyRun(TickData data)
        {
            var mainPad = ControllerManager.instance.GetMain();
            if (!mainPad.IsValid || !mainPad.IsConnected)
            {
                return BTresult.Failure;
            }

            var pressedButtons = mainPad.GetPad().GetPressedButtons();
            if (pressedButtons.Length == 0)
            {
                return BTresult.Running;
            }

            var binds = ModEntry.Preferences.KeyBindings[this.button];
            ModEntry.Preferences.KeyBindings[this.button] = this.Poll(binds, pressedButtons[0]);
            return BTresult.Success;
        }

        private int[] Poll(int[] array, int num)
        {
            if (array == null)
            {
                array = new int[this.orderIndex + 1];
                for (var i = 0; i < array.Length; i++)
                {
                    array[i] = i == this.orderIndex ? num : -1;
                }
            }
            else if (array.Length > this.orderIndex)
            {
                array[this.orderIndex] = num;
            }
            else if (array.Length - 1 < this.orderIndex)
            {
                var array2 = new int[this.orderIndex + 1];
                for (var j = 0; j < array2.Length; j++)
                {
                    if (j < array.Length)
                    {
                        array2[j] = array[j];
                    }
                    else if (j == this.orderIndex)
                    {
                        array2[j] = num;
                    }
                    else
                    {
                        array2[j] = -1;
                    }
                }

                array = array2;
            }

            for (var k = 0; k < array.Length; k++)
            {
                for (var l = array.Length - 1; l > k; l--)
                {
                    if (array[k] != array[l])
                    {
                        continue;
                    }

                    var array3 = new int[array.Length - 1];
                    for (var m = 0; m < array.Length; m++)
                    {
                        if (m < l)
                        {
                            array3[m] = array[m];
                        }
                        else if (m > l)
                        {
                            array3[m - 1] = array[m];
                        }
                    }

                    array = array3;
                }
            }

            return array;
        }
    }
}
