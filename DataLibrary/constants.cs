
namespace DataLibrary
{
    static class constants
    {
        public const float field = 1;
        public const int n = 3;

        public const string DefaultPath = "..\\..\\..\\..\\Files\\input.txt";

        public const string Format = "{0:0.000}";

        /* for serialization */
        public struct DataItemElems
        {
            public struct VecComponentsNames
            {
                public const string X = "DIVec_X";
                public const string Y = "DIVec_Y";               
            }
            public const string FieldName = "Field";
        }

        public struct DataOnGridElems
        {
            public struct VecComponentsNames
            {
                public const string vec = "Vec"; public const string x = "VecX"; public const string y = "VecY";


            }
            public const string GridName1 = "Grid1DX";
            public const string GridName2 = "Grid1DY";
            public const string InfoName = "Info";
            public const string DateName = "Date";
        }

        public struct MainCollectionElems
        {
            public const string ChangedAfterSaveName = "MCChgAfterSave";
            public const string CountName = "MCCount";
            
        }
    }
}
