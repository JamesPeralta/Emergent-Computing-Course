# Camera Movement
When viewing the game in play mode, press on the space bar to switch the angle.
You can view the game from the North, East, South, and West positions of the game floor.

# Custom Strategies

## Normal Behaviour
In the normal behaviour I have two urges for the snitch:
 1. Urge to move randomly towards a random point on the map.
 2. Urge to move away from the boundaries (North Wall, East Wall, South Wall, West Wall, Top Wall, Ground).

In the normal behaviour I have two urges for the players:
 1. Urge to move randomly towards the snitch.
 2. Urge to move away from another player if it is too close.

## Custom Behaviour 1
In the Custom Behaviour 1 I have three urges for the snitch:
 1. Urge to move randomly towards a random point on the map.
 2. Urge to move away from the boundaries (North Wall, East Wall, South Wall, West Wall, Top Wall, Ground).
 3. Urge to run away from players. To do this I find the nearest player and apply a force away from this player.

In the Custom Behaviour 1 I have 4 custom behaviours for the players:
 1. Urge to move randomly towards the snitch.
 2. Urge to move away from another player if it is too close.
 3. Urge to move towards the crowd.
 4. I added behaviour that if a player is close to the snitch, it will make a break towards it by doubling it's maxSpeed and MaxAcceleration. 
    It will also no longer care about swarming or avoiding nearby players so there will be more collisions.

Observed Behaviour:
Adding the extra urge for the snitch to avoid players makes the game alot harder for the players. I noticed that this caused them to score points
less frequently. Added urges to move towards the crowd gave the players a cool formation where they we're all clumped up a good distance away from 
the golden snitch. When I added the behaviour that players can now make a break towards the snitch by doubling it's speed and acceleration, I noticed
that they would clump up and then the players at the front would make a break for it which was interesting. However since the Gryffindor players were
faster, they would always push towards the front and the score most of the time which created a skew in the Gryffindors favour on the score card.

## Custom Behaviour 2
In the Custom Behaviour 2 I have three urges for the snitch:
 1. Urge to move randomly towards a random point on the map.
 2. Urge to move away from the boundaries (North Wall, East Wall, South Wall, West Wall, Top Wall, Ground).
 3. Urge to run away from players. To do this I find the nearest player and apply a force away from this player.

In the Custom Behaviour 2 I have 4 custom behaviours for the players:
 1. Urge to move randomly towards the snitch.
 2. Urge to move away from another player if it is too close.
 3. Urge to move towards the crowd.
 4. I added behaviour that if a Slytherin player is close to the snitch it will attempt to tackle Gryffindor players around him more often to push them out of the race.

Observed Behaviour:
When I added custom behaviour number 4, the Gryffindor players started getting tackled more often which resulted in the Slytherin players being ahead of the pack more often.
This was interesting because in the other rulesets the Gryffindor players were mostly ahead. This resulted Slytherin getting more points in the long run.
