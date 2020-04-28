using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;

namespace ANR
{
    public class WeatherManager : MonoBehaviour
    {
        #region Enum

        public enum Season
        {
            NONE,
            SPRING,
            SUMMER,
            AUTUMN,
            WINTER
        };

        public enum Weather
        {
            NONE,
            SUNNY,
            HOTSUN,
            RAIN,
            SNOW
        };

        #endregion

        #region Properties

        public Season currentSeason;
        public Weather currentWeather;

        public ParticleSystem rain;
        public ParticleSystem snow;

        [Header("Time Settings")] public float seasonTime;
        public float springTime;
        public float summerTime;
        public float autumnTime;
        public float winterTime;

        [Header("Light Settings")] public Light sunLight;
        public float summerLightIntensity;
        public float autumnLightIntensity;
        public float winterLightIntensity;
        private float defaultLightIntensity;
        public Color summerColor;
        public Color autumnColor;
        public Color winterColor;
        private Color defaultLightColor;

        #endregion

        // Use this for initialization
        private void Start()
        {
            this.currentSeason = Season.SPRING;
            this.currentWeather = Weather.SUNNY;

            this.seasonTime = this.springTime;

            this.defaultLightColor = this.sunLight.color;
            this.defaultLightIntensity = this.sunLight.intensity;

            this.rain.Stop();
            this.snow.Stop();
        }

        // Update is called once per frame
        private void Update()
        {
            // update season time
            this.seasonTime -= Time.deltaTime;

            // update season and weather
            if (this.currentSeason == Season.SPRING)
            {
                ChangeWeather(Weather.SUNNY);
                LerpSunIntensity(this.sunLight, defaultLightIntensity);
                LerpLightColor(this.sunLight, defaultLightColor);
                if (this.seasonTime <= 0f)
                {
                    ChangeSeason(Season.SUMMER);
                }
            }
            else if (this.currentSeason == Season.SUMMER)
            {
                ChangeWeather(Weather.HOTSUN);
                LerpSunIntensity(this.sunLight, summerLightIntensity);
                LerpLightColor(this.sunLight, summerColor);
                if (this.seasonTime <= 0f)
                {
                    ChangeSeason(Season.AUTUMN);
                }
            }
            else if (this.currentSeason == Season.AUTUMN)
            {
                ChangeWeather(Weather.RAIN);
                LerpSunIntensity(this.sunLight, autumnLightIntensity);
                LerpLightColor(this.sunLight, autumnColor);
                if (this.seasonTime <= 0f)
                {
                    ChangeSeason(Season.WINTER);
                }
            }
            else if (this.currentSeason == Season.WINTER)
            {
                ChangeWeather(Weather.SNOW);
                LerpSunIntensity(this.sunLight, winterLightIntensity);
                LerpLightColor(this.sunLight, winterColor);
                if (this.seasonTime <= 0f)
                {
                    ChangeSeason(Season.SPRING);
                }
            }
        }

        public void ChangeSeason(Season seasonType)
        {
            if (seasonType != this.currentSeason)
            {
                switch (seasonType)
                {
                    case Season.SPRING:
                        this.currentSeason = Season.SPRING;
                        this.seasonTime = this.springTime;
                        break;
                    case Season.SUMMER:
                        this.currentSeason = Season.SUMMER;
                        this.seasonTime = this.summerTime;
                        break;
                    case Season.AUTUMN:
                        this.currentSeason = Season.AUTUMN;
                        this.seasonTime = this.autumnTime;
                        break;
                    case Season.WINTER:
                        this.currentSeason = Season.WINTER;
                        this.seasonTime = this.winterTime;
                        break;
                }
            }
        }

        public void ChangeWeather(Weather weatherType)
        {
            if (weatherType != this.currentWeather)
            {
                switch (weatherType)
                {
                    case Weather.SUNNY:
                        this.currentWeather = Weather.SUNNY;
                        this.snow.Stop();
                        break;
                    case Weather.HOTSUN:
                        this.currentWeather = Weather.HOTSUN;
                        break;
                    case Weather.RAIN:
                        this.currentWeather = Weather.RAIN;
                        this.rain.Play();
                        break;
                    case Weather.SNOW:
                        this.currentWeather = Weather.SNOW;
                        this.rain.Stop();
                        this.snow.Play();
                        break;
                }
            }
        }

        private void LerpLightColor(Light light, Color color)
        {
            light.color = Color.Lerp(light.color, color, 0.2f * Time.deltaTime);
        }

        private void LerpSunIntensity(Light light, float intensity)
        {
            light.intensity = Mathf.Lerp(light.intensity, intensity, 0.2f * Time.deltaTime);
        }
    }
}