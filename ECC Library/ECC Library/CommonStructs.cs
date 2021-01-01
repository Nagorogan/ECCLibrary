﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Assets;

namespace ECCLibrary
{
    public struct EatableData
    {
        public bool CanBeEaten;
        public float FoodAmount;
        public float WaterAmount;
        public bool Decomposes;

        public EatableData(bool canBeEaten, float foodAmount, float waterAmount, bool decomposes)
        {
            CanBeEaten = canBeEaten;
            FoodAmount = foodAmount;
            WaterAmount = waterAmount;
            Decomposes = decomposes;
        }

        public Eatable MakeItemEatable(GameObject go)
        {
            if(go.GetComponent<Eatable>() != null)
            {
                ErrorMessage.AddMessage(string.Format("ECC: Object {0} already is edible.", go.name));
            }
            var eatable = go.AddComponent<Eatable>();
            eatable.allowOverfill = true;
            eatable.foodValue = FoodAmount;
            eatable.waterValue = WaterAmount;
            eatable.kDecayRate = 0.015f;
            eatable.SetDecomposes(Decomposes);
            return eatable;
        }
    }
    public struct ScannableItemData
    {
        public bool scannable;
        public float scanTime;
        public string encyPath;
        public string[] encyNodes;
        public Sprite popup;
        public Texture2D encyImage;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="scannable">Whether this can be scanned and has an encyclopedia entry.</param>
        /// <param name="scanTime">How long it takes to scan this creature.</param>
        /// <param name="encyPath">The path to the encyclopedia. Example: "Lifeforms/Fauna/Carnivores".</param>
        /// <param name="encyNodes">The path to the encyclopedia in array form. Example: { "Lifeforms", "Fauna", "Carnivores" }.</param>
        /// <param name="popup">The popup image. Must be exported as a Sprite. If null, the default popup is used.</param>
        /// <param name="encyImage">The image of the encyclopedia entry. If null, no image will be used.</param>
        public ScannableItemData(bool scannable, float scanTime, string encyPath, string[] encyNodes, Sprite popup, Texture2D encyImage)
        {
            this.scannable = scannable;
            this.scanTime = scanTime;
            this.encyPath = encyPath;
            this.encyNodes = encyNodes;
            this.popup = popup;
            this.encyImage = encyImage;
        }

        internal void AttemptPatch(ModPrefab prefab, string encyTitle, string encyDesc)
        {
            PDAEncyclopediaHandler.AddCustomEntry(new PDAEncyclopedia.EntryData()
            {
                key = prefab.ClassID,
                nodes = encyNodes,
                path = encyPath,
                image = encyImage,
                popup = popup
            });
            PDAHandler.AddCustomScannerEntry(new PDAScanner.EntryData()
            {
                key = prefab.TechType,
                encyclopedia = prefab.ClassID,
                scanTime = scanTime,
                isFragment = false
            });
            LanguageHandler.SetLanguageLine("Ency_" + prefab.ClassID, encyTitle);
            LanguageHandler.SetLanguageLine("EncyDesc_" + prefab.ClassID, encyDesc);
        }
    }
    public struct UBERMaterialProperties
    {
        public float Shininess;
        public float SpecularInt;
        public float EmissionScale;

        public UBERMaterialProperties(float shininess, float specularInt = 1f, float emissionScale = 1f)
        {
            Shininess = shininess;
            SpecularInt = specularInt;
            EmissionScale = emissionScale;
        }
    }
}
