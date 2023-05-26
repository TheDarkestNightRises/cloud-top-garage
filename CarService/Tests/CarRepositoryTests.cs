using CarService.Data;
using CarService.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CarService.Tests;

public class CarRepositoryTests : IDisposable
{
    private readonly DbContextOptions<AppDbContext> _options;

    public CarRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    

    public void Dispose()
    {

    }
}

