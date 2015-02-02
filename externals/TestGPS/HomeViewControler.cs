using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Web.Services;

using MonoTouch.CoreLocation;
using TestGPS.www.asianintroductions.co.uk;
using System.Text; 
using System.IO;
using System.Collections.Generic;
using MonoTouch.MapKit;
using ServiceStack.Text;

namespace TestGPS
{


	public class ReverseGeoCoder:MKReverseGeocoderDelegate{
		public ReverseGeoCoder(CLLocation location){

		}
		public override void FailedWithError (MKReverseGeocoder geocoder, NSError error){
			Console.WriteLine ("errrrrrr==="+error.LocalizedDescription);
		}
		public override void FoundWithPlacemark (MKReverseGeocoder geocoder, MKPlacemark placemark){
			var ob = placemark.AddressDictionary;
			Console.WriteLine ("addressssssss====={0}", ob);


			/*NSDictionary  *ditionary=[placemark addressDictionary];


			NSUserDefaults  *setting=[NSUserDefaults standardUserDefaults];

			NSString  *zip=[ditionary objectForKey:@"ZIP"];
			;

			self.zipfield.text=zip;

			if(zip)
				[setting setValue:zip forKey:@"ZipCode"];
			else{
				Alert(@"No zip code found");
			}
			*/

		}
	}
	class MyMapDelegate : MKMapViewDelegate
	{
		string pId = "PinAnnotation";
		string mId = "MonkeyAnnotation";

		public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, NSObject annotation)
		{
			MKAnnotationView anView;

			if (annotation is MKUserLocation)
				return null; 

			if (annotation is MonkeyAnnotation) {

				// show monkey annotation
				anView = mapView.DequeueReusableAnnotation (mId);

				if (anView == null)
					anView = new MKAnnotationView (annotation, mId);

				//anView.Image = UIImage.FromFile ("monkey.png");
				anView.CanShowCallout = true;
				anView.Draggable = true;
				anView.RightCalloutAccessoryView = UIButton.FromType (UIButtonType.DetailDisclosure);

			} else {

				// show pin annotation
				anView = (MKPinAnnotationView)mapView.DequeueReusableAnnotation (pId);

				if (anView == null)
					anView = new MKPinAnnotationView (annotation, pId);

				((MKPinAnnotationView)anView).PinColor = MKPinAnnotationColor.Red;
				anView.CanShowCallout = true;
			}

			return anView;
		}

		public override void CalloutAccessoryControlTapped (MKMapView mapView, MKAnnotationView view, UIControl control)
		{
			var monkeyAn = view.Annotation as MonkeyAnnotation;

			if (monkeyAn != null) {
				var alert = new UIAlertView ("Monkey Annotation", monkeyAn.Title, null, "OK");
				alert.Show ();
			}
		}

		public override MKOverlayView GetViewForOverlay (MKMapView mapView, NSObject overlay)
		{
			var circleOverlay = overlay as MKCircle;
			var circleView = new MKCircleView (circleOverlay);
			circleView.StrokeColor = UIColor.Black;
			circleView.FillColor = UIColor.Red;
			circleView.Alpha = 0.4f;
			return circleView;
		}
	}

	public class mapDel:MKMapViewDelegate{

		public mapDel(HomeViewControler controller){


		}
		public override MKOverlayView GetViewForOverlay (MKMapView mapView, NSObject overlay)
		{


			MKCircle c = (MKCircle)overlay;
			MKCircleView circleView = new MKCircleView (c);

			circleView.StrokeColor = UIColor.Red;
			circleView.FillColor = UIColor.Red;
			circleView.Alpha = 0.5f;
			return circleView;
		}
		public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, NSObject annotation)
		{

			if(annotation.GetType()==typeof(RegionAnnotation)){

				RegionAnnotation currentAnnotation = (RegionAnnotation)annotation;

				RegionAnnotationView regionView = (RegionAnnotationView )mapView.DequeueReusableAnnotation("ab");
				if (regionView == null) {
					regionView = new RegionAnnotationView (currentAnnotation,"ab",mapView);
					regionView.map = mapView;
				} else {
					regionView.theAnnotation = currentAnnotation;
					regionView.theAnnotation = currentAnnotation;
				}
				regionView.updateRadiusOverlay();

				return regionView;	
				}	

			return null;
			MKAnnotationView pinView;
			string pId = "";
			if (annotation is MKUserLocation)
				return null; 

			// create pin annotation view
			 pinView = (MKPinAnnotationView)mapView.DequeueReusableAnnotation (pId);

			if (pinView == null)
				pinView = new MKPinAnnotationView (annotation, pId);

			((MKPinAnnotationView)pinView).PinColor = MKPinAnnotationColor.Red;
			pinView.CanShowCallout = true;

			return pinView;
		}
	}
	public class TableView:UITableViewSource{

		HomeViewControler  control{ get; set;}
		public TableView(HomeViewControler controller){
			this.control = controller;
		}
		public override int NumberOfSections (UITableView tableView)
		{
			return 2;
		}

		public override int RowsInSection (UITableView tableview, int section){
			return 0;
		}


	
		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			if (section == 0)
				return control.locationViewP;
			if (section == 1)
				return control.mapViewP;
			return null;

		}

		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			string cellid = "";

			UITableViewCell cell = tableView.DequeueReusableCell (cellid);
			// if there are no cells to reuse, create a new one
			if (cell == null)
				cell = new UITableViewCell (UITableViewCellStyle.Default, cellid);

			return cell;
		}


		public override float GetHeightForHeader (UITableView tableView, int section)
		{
			if(section==0){
				return 400;
			}
			else{
				return 300;
			}
		}




	
	}
	public  delegate void  serviceCompleted(object ob1,object ob2);

	public class TextFieldDlegate:UITextFieldDelegate{
		public HomeViewControler  controller{ get; set;}
		public TextFieldDlegate(HomeViewControler controller){
			this.controller = controller;
		}
		public override bool ShouldReturn (UITextField textField)
		{
			textField.ResignFirstResponder();
			return true;
		}

		public override void EditingEnded (UITextField textField)
		{

		}
		public override void EditingStarted (UITextField textField)
		{
			if (textField == this.controller.accuracyFiieldP) {
				textField.InputView = controller.pickerP;
				textField.InputAccessoryView = this.controller.toolBrP;
				this.controller.currentField = textField;
				controller.pickerP.Model = new HomeViewControler.pickerDelegate (controller, new string[] {
					"BestForNavigation",
					"AccuracyBest",
					"NearestTenMeters",
					"HundredMeters",
					"ThreeKilometers"
				}, textField);
			} else {
				textField.InputView = controller.pickerP;
				textField.InputAccessoryView = this.controller.toolBrP;
				this.controller.currentField = textField;
				controller.pickerP.Model = new HomeViewControler.pickerDelegate (controller, new string[] {
					"5",
					"10",
					"15",
					"20",
					"25"
				}, textField);
			}
		}

	}

	public class CoreLocationDelegate:CLLocationManagerDelegate{

		public HomeViewControler controller{ get; set;}
		public CoreLocationDelegate (HomeViewControler home)
		{
			this.controller = home;
		}
		public override void AuthorizationChanged (CLLocationManager manager, CLAuthorizationStatus status)
		{

		}

		public override void DeferredUpdatesFinished (CLLocationManager manager, NSError error)
		{

		}

		public override void DidStartMonitoringForRegion (CLLocationManager manager, CLRegion region)
		{

		}



		public override void Failed (CLLocationManager manager, NSError error)
		{
			Console.WriteLine ("errorrrrrr"+error.LocalizedDescription);
		}
	    public void callTaskAfternumberofMinut(int minut,CLLocation newLocation){
			DateTime newLocationTimestamp = this.NSDateToDateTime (newLocation.Timestamp);
			NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;

			DateTime? lastLocationUpdateTiemstamp=null;

			if (userDefaults != null) {

				NSDate last = (NSDate)userDefaults.ValueForKey (new NSString("last"));

				if (last != null)

					lastLocationUpdateTiemstamp = this.NSDateToDateTime (last);

				TimeSpan t = TimeSpan.MaxValue;
				if (lastLocationUpdateTiemstamp != null)
					t = (TimeSpan)(newLocationTimestamp - lastLocationUpdateTiemstamp);



				if (t.TotalSeconds > (60*(Convert.ToInt32(this.controller.trackMinuteP.Text)))){

					NSDate newDate = newLocation.Timestamp;

					this.controller.PerformSelector (new MonoTouch.ObjCRuntime.Selector("callLocation:"), newLocation, 1.0);
					userDefaults.SetValueForKey (newDate, new NSString ("last"));


				}
			}

		}
		public override void LocationsUpdated (CLLocationManager manager, CLLocation[] locations)
		{
			/*
			MKReverseGeocoder *geoCoder = [[MKReverseGeocoder alloc]     
			                               initWithCoordinate:newLocation.coordinate ];
			geoCoder.delegate = self;
			[geoCoder start];
			*/
			//Called in the case of ios 6 

			Console.WriteLine ("UpdatedHeading======{0}",locations);
			CLLocation newLocation=locations[locations.Length-1];
			MKReverseGeocoder geoCoder = new MKReverseGeocoder (newLocation.Coordinate);
			geoCoder.Delegate = new ReverseGeoCoder (newLocation);

			controller.latestLocation=newLocation;

			//Check app is in background or foreground
			
			if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active) {
				this.controller.latitudeLblP.Text=newLocation.Coordinate.Latitude.ToString();
				this.controller.longitudeLblP.Text=newLocation.Coordinate.Longitude.ToString();;
				this.controller.altitudeLblP.Text=newLocation.Altitude.ToString();;
				this.controller.sppeedP.Text=newLocation.Speed.ToString();;
				this.controller.carseP.Text=newLocation.Course.ToString();
				this.controller.verticalAccuracyP.Text= newLocation.HorizontalAccuracy.ToString();;
				this.controller.timeStampP.Text = newLocation.Timestamp.ToString();
				this.controller.horizontalAccuracyP.Text = newLocation.HorizontalAccuracy.ToString();
				int minut = Convert.ToInt32 (this.controller.trackMinuteP.Text);

				//////// Save location after given minut
				callTaskAfternumberofMinut (minut, newLocation);



			} else {


				int minut = Convert.ToInt32 (this.controller.trackMinuteP.Text);
				callTaskAfternumberofMinut (minut, newLocation);
			}
		


		}

		public override void LocationUpdatesPaused (CLLocationManager manager)
		{
			Console.WriteLine ("LocationUpdatesPaused======");
		}

		public override void LocationUpdatesResumed (CLLocationManager manager)
		{

		}

		public override void MonitoringFailed (CLLocationManager manager, CLRegion region, NSError error)
		{
			Console.WriteLine ("MonitoringFailed======");
		}

		public override void RegionEntered (CLLocationManager manager, CLRegion region)
		{
			Console.WriteLine ("RegionEntered======{0}",region);
		}

		public override void RegionLeft (CLLocationManager manager, CLRegion region)
		{
			Console.WriteLine ("RegionLeft======{0}",region);
		}

		public override bool ShouldDisplayHeadingCalibration (CLLocationManager manager)
		{
			Console.WriteLine ("ShouldDisplayHeadingCalibration======{0}",manager);
			return true;
		}

		public override void UpdatedHeading (CLLocationManager manager, CLHeading newHeading)
		{
			Console.WriteLine ("UpdatedHeading======{0}......{0}",manager,newHeading);
		}
		public  DateTime NSDateToDateTime(MonoTouch.Foundation.NSDate date)
		{
			return (new DateTime(2001,1,1,0,0,0)).AddSeconds(date.SecondsSinceReferenceDate);
		}
		public  MonoTouch.Foundation.NSDate DateTimeToNSDate(DateTime date)
		{
			return MonoTouch.Foundation.NSDate.FromTimeIntervalSinceReferenceDate((date-(new DateTime(2001,1,1,0,0,0))).TotalSeconds);
		}
		public override void UpdatedLocation (CLLocationManager manager, CLLocation newLocation, CLLocation oldLocation)
		{
		//Caled in case of ios 5

			if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active) {
				this.controller.latitudeLblP.Text=newLocation.Coordinate.Latitude.ToString();
				this.controller.longitudeLblP.Text=newLocation.Coordinate.Longitude.ToString();;
				this.controller.altitudeLblP.Text=newLocation.Altitude.ToString();;
				this.controller.sppeedP.Text=newLocation.Speed.ToString();;
				int minut = Convert.ToInt32 (this.controller.trackMinuteP.Text);
				callTaskAfternumberofMinut (minut, newLocation);


			} else {
				int minut = Convert.ToInt32 (this.controller.trackMinuteP.Text);
				callTaskAfternumberofMinut (minut, newLocation);

			    }
		}

	

	}


	public partial class HomeViewControler : UIViewController
	{
		public UITextField currentField{ get; set;}
		CLLocationManager locationManager{ get; set;}

		public    CLLocation latestLocation{ get; set;}
		public MonoTouch.UIKit.UIView mapViewP { get; set; }


		public MonoTouch.MapKit.MKMapView mapP { get; set; }


		public MonoTouch.UIKit.UILabel timeStampP { get; set; }

	
		public 	MonoTouch.UIKit.UIView locationViewP { get; set; }

	
		public MonoTouch.UIKit.UILabel horizontalAccuracyP { get; set; }


		public MonoTouch.UIKit.UILabel carseP { get; set; }

	
		public MonoTouch.UIKit.UILabel verticalAccuracyP { get; set; }


		public MonoTouch.UIKit.UIToolbar toolBrP { get; set; }

		[Outlet]
		public MonoTouch.UIKit.UIPickerView pickerP { get; set; }


		public MonoTouch.UIKit.UISwitch serviceSwitchP { get; set; }


		public MonoTouch.UIKit.UILabel labelP { get; set; }


		public MonoTouch.UIKit.UITextField accuracyFiieldP { get; set; }


		public MonoTouch.UIKit.UITextField trackMinuteP { get; set; }

	
		public MonoTouch.UIKit.UILabel latitudeLblP { get; set; }

	
		public MonoTouch.UIKit.UILabel longitudeLblP { get; set; }


		public MonoTouch.UIKit.UILabel altitudeLblP { get; set; }


		public MonoTouch.UIKit.UILabel sppeedP { get; set; }
		public MonoTouch.UIKit.UITableView locationTableP{ get; set;}
	
		public MonoTouch.UIKit.UIButton biginToUpdateP { get; set; }

		public HomeViewControler () : base ("HomeViewControler", null)
		{

		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		partial void donePickerPress (MonoTouch.Foundation.NSObject sender){

			currentField.ResignFirstResponder();
		}
		public void updateLocation(){
			beginLocation (null);
		}
 		partial void enabledStateChanged (MonoTouch.Foundation.NSObject sender){


			if(this.serviceSwitch.On){
				this.locationManager.StartUpdatingLocation();

			}
			else{
				this.locationManager.StopUpdatingLocation();
			}
		}
		partial void callLocation (MonoTouch.Foundation.NSObject sender){
			CLLocation l=(CLLocation)sender;

			UIAlertView alert=new UIAlertView(@"Alert","You can call here any method to save or send data in server.This will be called untill and unless location updates stops",null,"Ok",null);
			alert.Show();
			return;

			string s=String.Format("<Location><Latitude>{0}</Latitude><Longitude>{0}</Longitude><Altitude>{0}</Altitude><Speed>{0}</Speed><Coarse>{0}</Coarse><HorizontalAccuracy>{0}</HorizontalAccuracy><VerticalAccuracy>{0}</VerticalAccuracy><TimeStemp>{0}</TimeStemp></Location>", l.Coordinate.Latitude,l.Coordinate.Longitude,l.Altitude.ToString(),l.Speed.ToString(),
			                       l.Course.ToString(),l.HorizontalAccuracy.ToString(),l.VerticalAccuracy.ToString(),l.Timestamp.ToString());

			GpsWebServiceTest1WebService service = new GpsWebServiceTest1WebService ();
			service.AddGpsInformationAsync(s);
			//string ser = service.HelloWorld ();
			service.AddGpsInformationCompleted += new AddGpsInformationCompletedEventHandler (addGPSLocationSucceed);
		}
		public void saveSetting(CLLocation latestLocation){
			Setting s=new Setting();
			MyLocation ml=new MyLocation();
			ml.lat=latestLocation.Coordinate.Latitude;
			ml.lng=latestLocation.Coordinate.Longitude;
			ml.vaccuracy=latestLocation.VerticalAccuracy.ToString();
			ml.haccuracy=latestLocation.HorizontalAccuracy.ToString();
			ml.timeintrval=latestLocation.Timestamp;
			ml.coarse=latestLocation.Course;
			ml.altitude=latestLocation.Altitude;
			s.isAllow=true;
			s.location=ml;
			string json=JsonSerializer.SerializeToString<Setting>(s);
			string cachedFile = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.InternetCache),"Setting");
			File.WriteAllText(cachedFile,json);
			serviceSwitchP.SetState(s.isAllow,true);
		}
		partial void beginLocation (MonoTouch.Foundation.NSObject sender){
			MKCircle circleOverlay;
			MKCircleView circleView=null;

			if(latestLocation!=null){

				this.addRegion(latestLocation.Coordinate);

				saveSetting(latestLocation);
			}
			if(this.locationManager!=null){
			this.locationManager.StopUpdatingLocation();
			this.locationManager.StartUpdatingLocation ();
			}
		}
		public class pickerDelegate:UIPickerViewModel{
			public HomeViewControler homeViewControler{get;set;}
			public string [] pickerList{ get; set;}
			public UITextField targetField{ get; set;}

			public pickerDelegate(HomeViewControler controller,string [] list,UITextField targetField){
				this.homeViewControler=controller;
				this.targetField=targetField;
				this.pickerList=list;
				controller.currentField=targetField;
			}


			public override int GetComponentCount (UIPickerView picker)
			{
				return 1;
			}


			public override int GetRowsInComponent (UIPickerView picker, int component)
			{
				return pickerList.Length;
			}

			public override string GetTitle (UIPickerView picker, int row, int component)
			{
				return pickerList[row];
			}



			public override void Selected (UIPickerView picker, int row, int component)
			{
				homeViewControler.currentField.Text=pickerList[row];

			}
		}
		public void addRegion(CLLocationCoordinate2D coor) {
			//map.MapType = MKMapType.st;
			map.ShowsUserLocation = true;

			// set map center and region
			double lat = 42.374260;
			double lon = -71.120824;
			var mapCenter = coor;
			var mapRegion = MKCoordinateRegion.FromDistance (mapCenter, 3000, 3000);
			map.CenterCoordinate = mapCenter;
			map.Region = mapRegion;

			// add an annotation
			map.AddAnnotation (new MKPointAnnotation () {
				Title = "MyAnnotation", 
				Coordinate = coor
			});

			// set the map delegate

				map.Delegate = new MyMapDelegate ();;

			// add a custom annotation
			map.AddAnnotation (new MonkeyAnnotation ("Xamarin", mapCenter));

			// add an overlay
			var circleOverlay = MKCircle.Circle (mapCenter, 1000);
			if(map.Overlays!=null)
			map.RemoveOverlays (map.Overlays);

			map.AddOverlay (circleOverlay);
		}


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			accuracyFiield.Delegate = new TextFieldDlegate (this);
			trackMinute.Delegate = new TextFieldDlegate (this);

			this.locationManager = new CLLocationManager ();

			this.locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
			this.locationManager.Delegate = new CoreLocationDelegate(this);
			accuracyFiield.Text="Best";
			trackMinute.Text = "5";

			//trackMinute.InputView = picker;
			//trackMinute.InputAccessoryView = toolBr;
			//picker.Model =new  pickerDelegate (this, null,trackMinute);

			///////////////
			/// 
			/// 
			/// 

			this.Title = "Location Manager";
			this.NavigationController.NavigationBar.TintColor=UIColor.Gray;
			GpsWebServiceTest1WebService service = new GpsWebServiceTest1WebService ();




			//string ser = service.HelloWorld ();
//			service.GetGpsInformationPreviouslySent_Last1000RecordsAsync ();

//
//			service.GetGpsInformationPreviouslySent_Last1000RecordsCompleted += new GetGpsInformationPreviouslySent_Last1000RecordsCompletedEventHandler (getGpsprevios1000RecordsFound);
//          

		
			this.sppeedP = sppeed;
			this.locationViewP = locationView;
			this.accuracyFiieldP = accuracyFiield;
			this.latitudeLblP = latitudeLbl;//latitudeLblP
			this.labelP = label;
			this.longitudeLblP = longitudeLbl;
			this.locationTableP = locationTable;
			this.locationViewP = locationView;
			this.mapViewP = mapView;
			this.altitudeLblP = altitudeLbl;
			this.pickerP = picker;
			this.toolBrP = toolBr;
			this.serviceSwitchP=serviceSwitch;
			this.locationViewP = locationView;
			this.altitudeLblP = altitudeLbl;
			this.horizontalAccuracyP = horizontalAccuracy;
			this.verticalAccuracyP = verticalAccuracy;
			this.timeStampP = timeStamp;
			this.trackMinuteP = trackMinute;
			this.mapP = map;
			this.carseP = carse;
			this.locationTableP.Source=new TableView (this);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);


			string chache=Environment.GetFolderPath (Environment.SpecialFolder.InternetCache);

			string cachedFile = Path.Combine (chache,"Setting");
			if(File.Exists(cachedFile)){
				string data = File.ReadAllText (cachedFile);

				var sdata = JsonSerializer.DeserializeFromString<Setting> (data);
				this.latitudeLblP.Text = sdata.location.lat.ToString ();


				this.longitudeLblP.Text=sdata.location.lng.ToString();;
				this.altitudeLblP.Text=sdata.location.altitude.ToString();;
				this.sppeedP.Text=sdata.location.speed.ToString();;
				this.carseP.Text = sdata.location.coarse.ToString();
				this.verticalAccuracyP.Text= sdata.location.vaccuracy.ToString();;
				this.timeStampP.Text = sdata.location.timeintrval.ToString();
				this.horizontalAccuracyP.Text = sdata.location.haccuracy.ToString();
				this.addRegion (new CLLocationCoordinate2D(sdata.location.lat,sdata.location.lng));
				serviceSwitch.On = sdata.isAllow;
			}

//			if(latestLocation!=null){
//				mapP.Delegate=new mapDel(this);
//				mapP.AddAnnotation (new MKPointAnnotation (){
//					Title=latestLocation.Description(),
//					Coordinate = latestLocation.Coordinate
//				});
//			}

		}
		public void addGPSLocationSucceed(object a,AddGpsInformationCompletedEventArgs args){
			bool result = args.Result;

			if (result == true) {
				Console.WriteLine ("adding location to server succeed");
			} else {
				Console.WriteLine ("unable to send ");
			}
		}


		public void getGpsprevios1000RecordsFound(object a,GetGpsInformationPreviouslySent_Last1000RecordsCompletedEventArgs args){
			GPSListItem[]  s=args.Result;

			if (args.Cancelled) {
			} else if (args.Error != null) {

			} else {

				GetGpsInformationPreviouslySent_Last1000RecordsCompletedEventArgs arg=(GetGpsInformationPreviouslySent_Last1000RecordsCompletedEventArgs)args;
				GPSListItem[] Result=arg.Result;
				foreach(GPSListItem item in Result){
					Console.WriteLine(item.RecordID+"vvvvvvv"+item.XMLContent+"{0}",item.RecordCreated);
				}

			}
		}
	}

}

