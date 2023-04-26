# ChampionSynergy webapp

# Table of content
- Overview
- Prerequisites 
- Usage
- Things Learned. 

# Overview
Webapp that pulls match history/data, on the searched Summoner name/Profile name, from Riot's league of legends API and displays which characters on your team and the enemy team you win or lose the most against.

# Prerequisites 
you will need a windows machine, visual studio (not code) a browser and an Riot API key

# Usage
open the project in visual studio
- right click on the solution 
- select properties
- go under startup project, then do the following to start everything needed
1) select single startup project 
2) select the ChampionSynergy -> set to start
3) click apply or ok. 
4) Go into the ApiKeyExample.txt file in the Config folder in the RestSharpClient project and paste your API Key text into the file instead of the text that is there and hit ctrl + s 
4) click ctrl + f5 to start the project without debuggin. 

# Things i learned
- C#
- Web communication via http
- HTML & CSS
- MVC
- REST Api
- Rest Client
- Razor syntax
- Deserialization
