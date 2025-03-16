﻿
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by SkyHigh.PocoEntity.Generator version 1.0.0
//     Template version: 1.0.0
//
//     Generated at: 2025-03-16 17:08:22
//     Based on model: SkyHigh.EF.Demo [1.0.0]
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
        
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SkyHigh.EF.Demo.Domain.Entities
{
    /// <summary>
    /// Interface for entities that have timestamp properties.
    /// </summary>
    public interface ITimestampable
    {
        /// <summary>
        /// The date and time when the entity was created.
        /// </summary>
        DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The date and time when the entity was last modified.
        /// </summary>
        DateTimeOffset ModifiedAt { get; set; }
    }
}