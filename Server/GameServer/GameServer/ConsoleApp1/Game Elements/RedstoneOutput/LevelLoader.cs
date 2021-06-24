using System;
using System.Collections.Generic;
using System.Text;
using sharedAngy;

namespace Server
{
    class LevelLoader : RedstoneOutput
    {
        public string fileName;
        private string sceneName;
        public LevelLoader(GameRoom pRoom, int pX, int pY, int pZ, string pFileName, int pID, List<int> pActuators ) : base(pRoom, pX, pY, pZ, pID, CollInteractType.PASS, pActuators, false)
        {
            room = pRoom;
            string[] sceneNames = pFileName.Split(".txt");
            sceneName = sceneNames[0];
            fileName = "../../../../LevelFiles/" + pFileName;
            //room.PrintGrid(room.roomArray);
            room.roomArray[x(), y(), z()].Add(this);
            objectIndex = 14;
        }

        public override void CheckOutput()
        {
            base.CheckOutput();
            if (isActivated)
            {
                room.isReloading = true;
                room.levelFile = fileName;
                room.sceneName = sceneName;
            }
            
        }

        
    }
}

