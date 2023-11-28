using UnityEngine;

public class Gem : Pickup {

    #region Fields

    private float amount;

    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        base.Initialize();
        Type = PickupType.Gem;
        return true;
    }

    public void SetInfo(int amount) {
        this.amount = amount;
        if (amount < 3) {
            this.GetComponent<SpriteRenderer>().sprite = Main.Resource.Load<Sprite>($"Gem_Bronze.sprite");
            this.transform.localScale = Vector3.one;
        }
        else if (amount < 6) {
            this.GetComponent<SpriteRenderer>().sprite = Main.Resource.Load<Sprite>($"Gem_Silver.sprite");
            this.transform.localScale = Vector3.one;
        }
        else if (amount < 9) {
            this.GetComponent<SpriteRenderer>().sprite = Main.Resource.Load<Sprite>($"Gem_Gold.sprite");
            this.transform.localScale = Vector3.one;
        }
        else {
            this.GetComponent<SpriteRenderer>().sprite = Main.Resource.Load<Sprite>($"Gem_Gold.sprite");
            this.transform.localScale = Vector3.one * 1.25f;
        }
        
    }

    #endregion

    public override void GetPickup() {
        base.GetPickup();
    }

    public override void OnGetPickup() {
        base.OnGetPickup();

        Main.Object.Player.Exp += amount * Main.Game.CurrentWave.gemRatio;
        Main.Object.Despawn(this);
    }
}