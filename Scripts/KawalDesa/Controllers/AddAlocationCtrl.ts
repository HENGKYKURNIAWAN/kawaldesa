﻿/// <reference path="../../../Scaffold/Scripts/typings/angularjs/angular.d.ts"/>
/// <reference path="../../gen/Models.ts"/>
/// <reference path="IndexCtrl.ts"/>
/// <reference path="../KawalDesa.ts"/>


module App.Controllers {

    import Models = App.Models;
    import Controllers = App.Controllers.Models;

    class AddAllocationCtrl {

        static $inject = ["$scope"];

        indexCtrl: IndexCtrl;
        uploadDoc = new Models.DocumentUpload();
        file: any;

        constructor(public $scope) {
            var ctrl = this;
            this.indexCtrl = this.$scope.indexCtrl;

            $scope.$on('regionChangeSuccess', function () {
                ctrl.onRegionChanged();
            });
        }

        onRegionChanged() {
            if (this.indexCtrl.type == "add") {
                if (this.indexCtrl.region.Type <= 1) {
                    this.indexCtrl.configureDocumentUpload(Models.DocumentUploadType.NationalAdd, "0");
                } else if (this.indexCtrl.region.Type >= 2) {
                    var kab = this.indexCtrl.region;
                    while (kab.Type != 2) {
                        kab = kab.Parent;
                    }
                    this.indexCtrl.configureDocumentUpload(Models.DocumentUploadType.RegionalAdd, kab.Id);
                }
                this.getRecapitulations(this.indexCtrl.region.Id);
            }
        }

        getRecapitulations(parentId: string) {
            var ctrl = this;
            var scope = this.$scope;
            var query = {
                "SortOrder": "ASC",
                "fkParentId": parentId
            }
            var type : any = Controllers.FrozenNationalAddRecapitulationController;
            if (this.indexCtrl.currentUser) {
                type = Controllers.NationalAddRecapitulationController;
            }
            if (this.indexCtrl.region.Type >= 2) {
                type = Controllers.FrozenRegionalAddRecapitulationController;
                if (this.indexCtrl.currentUser) {
                    type = Controllers.RegionalAddRecapitulationController;
                }
            }
            type.GetAll(query).done((recapitulations) => {
                scope.$apply(() => {
                    scope.entities = recapitulations.filter(r => r.RegionId != parentId);
                    scope.total = recapitulations.filter(r => r.RegionId == parentId)[0];
                });
            });
        }


    }

    kawaldesa.controller("AddAllocationCtrl", AddAllocationCtrl);
}