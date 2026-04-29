using System;
using restaurant_management_system._2.Domain.Entities;
using restaurant_management_system._2.Infrastructure;

FileStorage storage = new FileStorage();

TableStorage db = storage.Load();

db.Tables.Add(new Table
{
    Id = db.NextId++,
    Number = 1,
    Capacity = 4,
    Location = "Main hall",
    IsOccupied = false
});

db.Tables.Add(new Table
{
    Id = db.NextId++,
    Number = 2,
    Capacity = 2,
    Location = "Window",
    IsOccupied = false
});

db.Tables.Add(new Table
{
    Id = db.NextId++,
    Number = 3,
    Capacity = 6,
    Location = "Terrace",
    IsOccupied = true
});

storage.Save(db);

Console.WriteLine("Tables saved successfully!");