﻿/// <reference path="../../../../Framework/Signum.Web/Signum/Scripts/globals.ts"/>
define(["require", "exports"], function(require, exports) {
    function init() {
        var $selectionOrigin;
        var currShiftSelection = [];
        var inSelectionProcess = false;
        var inactiveDivClass = "sf-cal-day-inactive";
        var selectedClass = "sf-cal-cell-selected";

        function selectMultiple($td) {
            $.each(currShiftSelection, function () {
                $(this).removeClass(selectedClass);
            });

            var originIndex = $selectionOrigin.index();
            var originTrIndex = $selectionOrigin.closest("tr").index();

            var index = $td.index();
            var trIndex = $td.closest("tr").index();

            var $calTableBody = $td.closest("tbody");

            var $currTr;
            var $currTd;
            for (var r = Math.min(originTrIndex, trIndex); r <= Math.max(originTrIndex, trIndex); r++) {
                $currTr = $calTableBody.children("tr:nth-child(" + (r + 1) + ")");
                for (var c = Math.min(originIndex, index); c <= Math.max(originIndex, index); c++) {
                    $currTd = $currTr.children("td:nth-child(" + (c + 1) + ")");
                    if ($currTd.children("." + inactiveDivClass).length == 0) {
                        $currTd.addClass(selectedClass);
                        currShiftSelection.push($currTd);
                    }
                }
            }
        }

        $(document).on("mousedown", ".sf-annual-calendar td:not(.sf-annual-calendar-month)", function (e) {
            var $this = $(this);

            if ($this.children("." + inactiveDivClass).length > 0) {
                return;
            }

            var rightClick = (e.which == 3);
            if (rightClick && $this.hasClass(selectedClass)) {
                return;
            }

            if (e.shiftKey) {
                selectMultiple($this);
            } else if (e.ctrlKey) {
                inSelectionProcess = true;
                $this.addClass(selectedClass);
                $selectionOrigin = $this;
                currShiftSelection = [];
            } else {
                inSelectionProcess = true;
                $("." + selectedClass).removeClass(selectedClass);
                $this.addClass(selectedClass);
                $selectionOrigin = $this;
                currShiftSelection = [];
            }

            e.preventDefault();
        });

        $(document).on("mouseover mouseup", ".sf-annual-calendar td:not(.sf-annual-calendar-month)", function (e) {
            var $this = $(this);
            if ($this.children("." + inactiveDivClass).length > 0) {
                return;
            }

            if (inSelectionProcess) {
                selectMultiple($this);
            }

            if (e.type == "mouseup") {
                inSelectionProcess = false;
            }

            e.preventDefault();
        });
        //$(".sf-annual-calendar-slider").slider({
        //    min: 40,
        //    max: 90,
        //    value: 80,
        //    step: 10,
        //    slide: function (event, ui) {
        //        $(".sf-annual-calendar td").width(ui.value + "px").height(ui.value + "px");
        //    }
        //});
    }
    exports.init = init;
});
//# sourceMappingURL=calendar.js.map
