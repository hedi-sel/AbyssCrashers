using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using AbyssCrashers.world.scripts.helpers;

public partial class FloorGenerator : FloorGeneratorBase
{
    [Export(PropertyHint.Dir)] public string MonsterSetsFolder;
    private Dictionary<int, MonsterSetResource> _monsterSets;

    private Dictionary<RoomId, RoomTemplate> _rooms = new()
    {
        { new RoomId(0, 0), RoomTemplate.StartingRoom },
        { new RoomId(-1, 0), RoomTemplate.MonsterRoom },
        { new RoomId(0, -1), RoomTemplate.MonsterRoom },
    };

    public override void _Ready()
    {
        base._Ready();

        // Needto define some kind of seed

        _monsterSets = MonsterSetResource.LoadMonsterSets(MonsterSetsFolder);

        foreach (var room in _rooms)
        {
            GenerateRoomAt(room.Key, 0, room.Value);
        }
    }

    private void GenerateRoomAt(RoomId room, int floor, RoomTemplate template)
    {
        var tileRect = room.GetTileRect();
        GenerateBaseRoom(tileRect, floor, Connections(room));
        foreach (var element in template.Elements)
        {
            GenerateElement(room, floor, element);
        }
    }

    private void GenerateElement(RoomId room, int floor, RoomElement element)
    {
        switch (element.Type)
        {
            case RoomElementType.PlayerSpawner:
                var baseTile = room.GetTileRect().Position - RoomId.RoomStartTile;
                SetCell(0, baseTile, floor, TileLibrary.PlayerSpawn);
                SetCell(0, baseTile + Vector2I.Up, floor, TileLibrary.PlayerSpawn);
                SetCell(0, baseTile + Vector2I.Left, floor, TileLibrary.PlayerSpawn);
                SetCell(0, baseTile + Vector2I.Up + Vector2I.Left, floor, TileLibrary.PlayerSpawn);
                break;
            case RoomElementType.Monster:
                if (!Multiplayer.IsServer()) break;
                var prefab = _monsterSets[floor].GetMonsterPrefab(((MonsterRoomElement)element).Monster);
                InstanceHolder.Get<MonsterSpawner>().SpawnOne(prefab, element.TilePosition, room);
                break;
        }
    }

    private List<CardinalDirection> Connections(RoomId room)
    {
        return new[] { CardinalDirection.Up, CardinalDirection.Down, CardinalDirection.Left, CardinalDirection.Right }
            .Where(dir => _rooms.ContainsKey(room.Next(dir))).ToList();
    }
}