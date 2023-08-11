using Colyseus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager> {
    [field: SerializeField] public LossCounter LossCounter { get; private set; }
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private EnemyController _enemy;
    public Action EnemyCreated;
    private ColyseusRoom<State> _room;
    private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();
    protected override void Awake() {
        base.Awake();

        Instance.InitializeClient();
        Connect();
    }
    private async void Connect(){
        Dictionary<string, object> data = new Dictionary<string, object>(){
            { "hp", _player.MaxHealth },
            { "speed", _player.Speed },
            { "spSqt", _player.SpeedSquating }
        };

        _room = await Instance.client.JoinOrCreate<State>("state_handler", data);
        _room.OnStateChange += OnChange;

        _room.OnMessage<string>("Shoot", ApplyShoot);
    }

    private void ApplyShoot(string jsonShootInfo) {
        ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);
        if(_enemies.ContainsKey(shootInfo.key) == false){
            Debug.LogError("Енеми нет, а он пытался стрелять");
            return;
        }
        _enemies[shootInfo.key].Shoot(shootInfo);

    }

    private void OnChange(State state, bool isFirstState) {
        if(!isFirstState) return;
       
        state.players.ForEach((key, player) => {
            if(key == _room.SessionId) CreatePlayer(player);
            else CreateEnemy(key, player);
        });

        _room.State.players.OnAdd += CreateEnemy;
        _room.State.players.OnRemove += RemoveEnemy;
    }
    private void CreatePlayer(Player player){
        var position = new Vector3(player.pX, player.pY, player.pZ);

        var playerCharacter = Instantiate(_player, position, Quaternion.identity);

        player.OnChange += playerCharacter.OnChange;

        _room.OnMessage<bool>("Restart", playerCharacter.GetComponent<Controller>().Restart);
    }

    private void CreateEnemy(string key, Player player){
        var position = new Vector3(player.pX, player.pY, player.pZ);

        var enemy = Instantiate(_enemy, position, Quaternion.identity);
        enemy.Init(key, player);

        _enemies.Add(key, enemy);
        EnemyCreated?.Invoke();
    }
    private void RemoveEnemy(string key, Player player){
        if(_enemies.ContainsKey(key) == false) return;
        var enemy = _enemies[key];

        enemy.Destroy();

        _enemies.Remove(key);
       
    }
    public void SendMessage(string key, Dictionary<string, object> data) => _room.Send(key, data);
    public void SendMessage(string key, string data){
        _room.Send(key,data);
    }

    public string GetSessionId() =>_room.SessionId;
    

    protected override void OnDestroy() {
        base.OnDestroy();
        _room.Leave();
    }
}
