angular
  .module('superhero')
  .controller("ShowController", function ($scope, Superhero, supersonic) {
    $scope.superhero = null;
    $scope.showSpinner = true;
    $scope.dataId = undefined;

    var _refreshViewData = function () {
      Superhero.find($scope.dataId).then( function (superhero) {
        $scope.$apply( function () {
          $scope.superhero = superhero;
          $scope.showSpinner = false;
        });
      });
    }

    supersonic.ui.views.current.whenVisible( function () {
      if ( $scope.dataId ) {
        _refreshViewData();
      }
    });

    supersonic.ui.views.current.params.onValue( function (values) {
      $scope.dataId = values.id;
      _refreshViewData();
    });

    $scope.remove = function (id) {
      $scope.showSpinner = true;
      $scope.superhero.delete().then( function () {
        supersonic.ui.layers.pop();
      });
    }
  });