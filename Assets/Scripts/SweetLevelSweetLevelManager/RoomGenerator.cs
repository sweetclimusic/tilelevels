namespace SweetLevelManager
{
    class RoomGenerator
    {
        //part of the splunky RoomGenerator
        int[] roomTypes = new int[]{0,1,2,3};
        int[] roomSolutionPath = new int[]{1,2,3,4,5};
        int[][] level = new int[18][]; //18X24
        int[][] room = new int[6][]; //6X8

        //roomTypes
        /*
            0: a side room that is not on the solution path
            1: a room that is guaranteed to have a left exit and a right exit
            2: a room that is guaranteed to have exits on the left, right, and bottom. 
            If there's another "2" room above it, then it also is guaranteed a top exit
            3: a room that is guaranteed to have exists on the left, right, and top
        */
    }
}