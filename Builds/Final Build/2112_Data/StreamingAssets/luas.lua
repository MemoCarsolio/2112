-- Main Proyect Code
--Author: Guillermo Carsolio Gonzalez A01700041


------General Glossary
-- e: Enemy
-- p: Player
-- point: point will be consider as x or y
-- dif: Diference
-- X,Y: is just x or y
--
--Remember that this script dictates the behaviour of the Enemy
--NOT the player



--General Helper Functions

--Function to get a value from an array of 2
function getCoordinate (table, point )
  if point == "x" then
    return table[1]
  end
  if point == "y" then
    return table[2]
  end
end


--Helper Lambda functions
greaterThanAnd = function(x,y,z)
  if x > y then
    if x > z then
      return true
    end
    return false
  end
end
--------------------------PathFinding------------------
-- In this function what we do is give as my input
-- X or Y values to compare in which direction
-- The enemy has to go

--enemy point change table


--Lambda functions that I use for pathFinding function

--Store Func In Variable
--                      input      valid           both posible outputs
getDirection = function(eP, pP) if eP < pP then return 1 end return -1 end
--
--
pathDiference = function(eP, pP) if eP > pP then return eP - pP end return pP - eP end

betterPath = function(difX,difY,maxDif)
  if greaterThanAnd(difX, difY, maxDif) then
    return "x"
  end
  if difY > maxDif then
    return "y"
  end
  return "both"
end
--PathFindingFunction
--Not sure if I can make this more efficent but tbh probably
function pathFinding(eX,eY,pX,pY,maxDif,point)
  local pointChange =  {0,0} --Set up a point that we will modify
  if betterPath(pathDiference(eX,pX),pathDiference(eY,pY), maxDif) == "x" then
   pointChange[1] = getDirection(eX,pX)
   pointChange[2] = 0 --So because the diference is larger we don't move in this direction
  end
  if betterPath(pathDiference(eX,pX),pathDiference(eY,pY), maxDif) == "y" then
   pointChange[1] = 0
   pointChange[2] = getDirection(eY,pY)
  end
  if betterPath(pathDiference(eX,pX),pathDiference(eY,pY), maxDif) == "both" then
   pointChange[1] = getDirection(eX,pX)
   pointChange[2] = getDirection(eY,pY)
  end
  if point == "x" then
   return getCoordinate(pointChange, "x")
  end
  if point == "y" then
   return getCoordinate(pointChange, "y")
  end
  return null
end


-----------------------Helper Functions for State Machine------------------------

--This function will return the min distance from either x or y perspective
function minDistance(eX,eY,pX,pY)
  local xpath = pathDiference(eX,pX)
  local ypath = pathDiference(eY,pY)
  if xpath > ypath then
    return xpath
  end
  return ypath
end


--Distant Attack Movement
--Some of the nex functions will use functions from path finding area
--e: enemey, c: Coodinate(it can be X or Y)
function keepDistance (eC,pC,distance)
  local dif = pathDiference(eC,pC)
  local dir = getDirection(eC,pC)
  if pathDiference(eC,pC) > distance then
    return dir
  end
  if pathDiference(eC,pC) < distance - 3 then --The -3 is so there is kind of a box where the Enemy can stay in peace
    return dir*-1
  end
  return 0 --So if we are at wanted distance we stay put
end

--This will be the patroling function
function patrol(p,dir,turn, limit)
  if turn < limit then
    return dir
  end
end
