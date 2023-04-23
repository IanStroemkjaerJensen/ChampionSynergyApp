# ChampionSynergy webapp

# Table of content
- Overview
- Prerequisites 
- Usage
- Things Learned. 

# Overview
Webapp that pulls match history/data, on the searched Summoner name/Profile name, from Riot's league of legends API and displays it, 
showing which characters on your team and enemies team you win/lose a lot against

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
4) Go into the MatchController.cs file in Controllers folder in the RestSharpClient project and paste your API key into the hard-coded _apiKey string variable at the
start of the file. Do the same in the SummonerController.cs file.
4) click ctrl + f5 to start the project without debuggin. 

# Things i learned
C#
Web communication via http
MVC
HTML & CSS
Razor syntax
REST api
