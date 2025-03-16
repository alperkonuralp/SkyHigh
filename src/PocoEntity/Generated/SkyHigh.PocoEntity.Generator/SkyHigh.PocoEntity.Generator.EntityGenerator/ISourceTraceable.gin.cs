﻿
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by SkyHigh.PocoEntity.Generator version 1.0.0
//     Template version: 1.0.0
//
//     Generated at: 2025-03-16 16:57:07
//     Based on model: SkyHigh.PocoEntity.Demo [1.0.0]
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
        
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SkyHigh.PocoEntity.Demo.Domain.Entities
{
    /// <summary>
    /// Interface for entities that are traceable to a source provider.
    /// </summary>
    public interface ISourceTraceable
    {
        /// <summary>
        /// The key of the source provider.
        /// </summary>
        string SourceProviderKey { get; set; }

        /// <summary>
        /// The identifier of the entity in the source provider.
        /// </summary>
        string SourceItemId { get; set; }

        /// <summary>
        /// The version of the entity in the source provider.
        /// </summary>
        string SourceVersion { get; set; }
    }
}