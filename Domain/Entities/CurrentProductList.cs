﻿using System;
using System.Collections.Generic;

namespace TechCareer.WernerHeisenberg.Northwind.Domain.Entities;

public partial class CurrentProductList
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;
}
