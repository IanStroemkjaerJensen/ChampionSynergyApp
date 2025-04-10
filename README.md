# ChampionSynergy webapp
Little for for fun project that takes your last 100 matches from your match history in league of legends from the Riot API and shows your top 10 champions you do well with and against from both teams and displays them in a table

# Table of content
- Overview
- Prerequisites 
- Usage
- Things Learned. 

# Overview
Webapp that pulls match history/data, on the searched Summoner name/Profile name, from Riot's league of legends API and displays which characters on your team and the enemy team you win or lose the most against, making it easier to make decisions in lobbies 

![image](https://github.com/IanStroemkjaerJensen/ChampionSynergyApp/assets/82367076/748cc681-61b1-4e9d-a3bc-6ab155553be9)


# Prerequisites 
You will need a windows machine, visual studio (not code) a browser and a Riot API key that you input in the config file

# Usage
open the project in visual studio
- right click on the solution 
- select properties
- go under startup project, then do the following to start everything needed
1) select single startup project 
2) select the ChampionSynergy -> set to start
3) click apply or ok. 
4) Go into the ApiKeyExample.txt file in the Config folder in the RestSharpClient project and paste your API key into the file instead of the key example that is there and hit ctrl + s 
4) click ctrl + f5 to start the project without debuggin. 

# Things i learned about
- C#
- Web communication via http
- MVC
- HTML & CSS
- REST Api
- Rest Client
- Deserialization
