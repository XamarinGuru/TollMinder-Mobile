using System;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;

namespace TestGPS
{
	public class RegionAnnotationView:MKPinAnnotationView
	{
		public MKMapView map{ get; set;}
		MKCircle radiusOverlay{ get; set;}
		public double  Radius;
		bool isRadiusUpdated;
		public RegionAnnotation theAnnotation{ get; set;}

		public RegionAnnotationView(MKAnnotation annotation,string str,MKMapView m):base(annotation,str)
		{

			this.CanShowCallout = true;;		
			this.MultipleTouchEnabled = true;
			this.Draggable = true;
			this.AnimatesDrop = true;
			this.map = m;
			this.theAnnotation = (RegionAnnotation )annotation;
			this.PinColor = MKPinAnnotationColor.Purple;
			radiusOverlay = MKCircle.Circle(theAnnotation.coordinate,theAnnotation.Radius);
			map.AddOverlay (radiusOverlay);
			//[map addOverlay:radiusOverlay];
		}
		public void updateRadiusOverlay(){
			if (!isRadiusUpdated) {
				isRadiusUpdated = true;

				this.removeRadiusOverlay();	

				this.CanShowCallout= false;

				map.AddOverlay(MKCircle.Circle(theAnnotation.Coordinate,Radius));	
				              
				               this.CanShowCallout = true;		
			}

		}
		public void removeRadiusOverlay(){

		}
	}
}

