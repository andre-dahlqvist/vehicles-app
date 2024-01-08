﻿using System.ComponentModel.DataAnnotations;

namespace VehiclesApi.Models;

public class Equipment
{
	public int Id { get; set; }

	[Required]
	[MaxLength(200)]
	public string Name { get; set; } = string.Empty;
}
