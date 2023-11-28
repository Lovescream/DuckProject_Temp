using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class ObjectManager {

    public Player Player { get; private set; }
    public List<Enemy> Enemies { get; private set; } = new();
    public HashSet<Gem> Gems => new();
    public HashSet<Pickup> Pickups => new();
    public HashSet<Projectile> Projectiles => new();
    //public HashSet<Bullet1> Bullets => new();

    public Transform EnemyParent {
        get {
            GameObject root = GameObject.Find("@Enemies");
            if (root == null) root = new("@Enemies");
            return root.transform;
        }
    }

    public void Clear() {
        Enemies.Clear();
        Gems.Clear();
        Pickups.Clear();
        Projectiles.Clear();
    }

    public void LoadMap(string mapName) {
        GameObject mapObject = Main.Resource.Instantiate(mapName);
        mapObject.transform.position = Vector3.zero;
        mapObject.name = "@Map";
        mapObject.GetComponent<Map>().Initialize();
    }

    public T Spawn<T>(string key, Vector2 position) where T : Thing {
        System.Type type = typeof(T);

        if (type == typeof(Player)) {
            GameObject obj = Main.Resource.Instantiate("Player.prefab");
            obj.transform.position = position;

            Player = obj.GetOrAddComponent<Player>();
            Player.SetInfo(key);

            return Player as T;
        }
        else if (type == typeof(Enemy)) {
            CreatureData data = Main.Data.Creatures[key];
            GameObject obj = Main.Resource.Instantiate($"{data.prefabName}.prefab", pooling: true);
            obj.transform.position = position;

            Enemy enemy = obj.GetOrAddComponent<Enemy>();
            enemy.SetInfo(key);
            Enemies.Add(enemy);

            return enemy as T;
        }
        else if (type == typeof(EliteEnemy)) {
            CreatureData data = Main.Data.Creatures[key];
            GameObject obj = Main.Resource.Instantiate($"{data.prefabName}.prefab", pooling: true);
            obj.transform.position = position;

            Enemy enemy = obj.GetOrAddComponent<EliteEnemy>();
            enemy.SetInfo(key);
            Enemies.Add(enemy);

            return enemy as T;
        }
        else if (type == typeof(Boss)) {
            CreatureData data = Main.Data.Creatures[key];
            GameObject obj = Main.Resource.Instantiate($"{data.prefabName}.prefab", pooling: true);
            obj.transform.position = position;

            Type classType = Type.GetType(key) ?? Type.GetType("Boss");
            Enemy enemy = obj.AddComponent(classType) as Enemy;
            enemy.SetInfo(key);
            Enemies.Add(enemy);

            return enemy as T;
        }
        else if (type == typeof(Gem)) {
            GameObject obj = Main.Resource.Instantiate("Gem.prefab", pooling: true);
            obj.transform.position = position;

            Gem gem = obj.GetOrAddComponent<Gem>();
            Gems.Add(gem);
            Main.Game.CurrentMap.Add(gem);

            return gem as T;
        }
        else if (type == typeof(Bomb)) {
            GameObject obj = Main.Resource.Instantiate("Bomb.prefab", pooling: true);
            obj.transform.position = position;

            Bomb bomb = obj.GetOrAddComponent<Bomb>();
            Pickups.Add(bomb);
            Main.Game.CurrentMap.Add(bomb);

            return bomb as T;
        }
        else if (type == typeof(TreasureChest)) {
            GameObject obj = Main.Resource.Instantiate("TreasureChest.prefab", pooling: true);
            obj.transform.position = position;

            TreasureChest chest = obj.GetOrAddComponent<TreasureChest>();
            Pickups.Add(chest);
            Main.Game.CurrentMap.Add(chest);

            return chest as T;
        }
        else if (type == typeof(Projectile)) {
            GameObject obj = Main.Resource.Instantiate(key, pooling: true);
            obj.transform.position = position;

            Projectile projectile = obj.GetOrAddComponent<Projectile>();
            Projectiles.Add(projectile);
            
            return projectile as T;
        }
        else if (type == typeof(Balls)) {
            GameObject obj = Main.Resource.Instantiate("Balls.prefab", pooling: true);
            obj.transform.position = position;

            Balls balls = obj.GetOrAddComponent<Balls>();
            return balls as T;
        }
        return null;
    }

    public void ShowDamageText(Vector2 position, float damage) {
        GameObject obj = Main.Resource.Instantiate("DamageText.prefab", pooling: true);
        DamageText text = obj.GetOrAddComponent<DamageText>();
        text.SetInfo(position, damage);
    }

    public void Despawn<T>(T obj) where T : Thing {
        System.Type type = typeof(T);

        if (type == typeof(Player)) {

        }
        else if (type == typeof(Enemy)) {
            Enemies.Remove(obj as Enemy);
            Main.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(EliteEnemy)) {
            Enemies.Remove(obj as EliteEnemy);
            Main.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(Boss)) {
            Enemies.Remove(obj as Boss);
            Main.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(Gem)) {
            Gems.Remove(obj as Gem);
            Main.Resource.Destroy(obj.gameObject);
            Main.Game.CurrentMap.Remove(obj as Gem);
        }
        else if (type == typeof(Bomb)) {
            Pickups.Remove(obj as Bomb);
            Main.Resource.Destroy(obj.gameObject);
            Main.Game.CurrentMap.Remove(obj as Bomb);
        }
        else if (type == typeof(TreasureChest)) {
            Pickups.Remove(obj as TreasureChest);
            Main.Resource.Destroy(obj.gameObject);
            Main.Game.CurrentMap.Remove(obj as TreasureChest);
        }
        else if (type == typeof(Projectile)) {
            Projectiles.Remove(obj as Projectile);
            Main.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(Balls)) {
            Main.Resource.Destroy(obj.gameObject);
        }
    }



    public List<Enemy> GetNearestEnemy(int count = 1, int distanceThreshold = 0) {
        List<Enemy> list = Enemies.OrderBy(x => (Player.transform.position - x.transform.position).sqrMagnitude).ToList();
        if (distanceThreshold > 0)
            list = list.Where(x => (Player.transform.position - x.transform.position).magnitude > distanceThreshold).ToList();

        List<Enemy> nearests = list.Take(Mathf.Min(count, list.Count)).ToList();
        if (nearests.Count == 0) return null;

        while (nearests.Count < count)
            nearests.Add(nearests.Last());

        return nearests;
    }
}

public enum PickupType {
    Gem,
    Bomb,
    TreasureChest,
}
public enum GemType {
    Bronze,
    Silver,
    Gold,
}