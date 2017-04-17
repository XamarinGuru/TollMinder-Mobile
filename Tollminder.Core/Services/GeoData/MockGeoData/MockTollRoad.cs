//using System;
//using System.Collections.Generic;
//using Tollminder.Core.Services.ProfileData;
//using Tollminder.Core.Models;

//namespace Tollminder.Core.Services.GeoData.MockGeoData
//{
//    public class MockTollRoad : ILoadResourceData<TollRoad>
//    {
//        private TollRoad mockRoad;
//        private List<TollPoint> mockPoints;

//        public MockTollRoad()
//        {
//            mockPoints = new List<TollPoint>();
//        }

//        public List<TollRoad> GetData(string fileName)
//        {

//        }

//        public TollRoad GetMockRoad()
//        {
//            mockPoints.Add(new TollPoint(){
//                Latitude = "",
//                Longitude = "",


//            });
//            mockRoad = new TollRoad("", );

//            return mockRoad;
//        }
//    }
//}
