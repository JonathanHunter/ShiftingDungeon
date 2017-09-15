namespace ShiftingDungeon.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class MoneyDisplay : MonoBehaviour
    {
        [SerializeField]
        private Text goldDisp;

        private void Update()
        {
            goldDisp.text = Character.Hero.HeroData.Instance.money.ToString();
        }
    }
}
