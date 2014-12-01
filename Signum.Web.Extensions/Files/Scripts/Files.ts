﻿/// <reference path="../../../../Framework/Signum.Web/Signum/Scripts/globals.ts"/>

import Entities = require("Framework/Signum.Web/Signum/Scripts/Entities")
import Lines = require("Framework/Signum.Web/Signum/Scripts/Lines")

export enum DownloadBehaviour {
    SaveAs,
    View,
    None
}

export interface FileLineOptions extends Lines.EntityBaseOptions {
    downloadUrl?: string;
    uploadUrl?: string;
    uploadDroppedUrl?: string;
    asyncUpload?: boolean;
    dragAndDrop?: boolean;
    download: DownloadBehaviour;
    fileType: string;
}

export interface FileAsyncUploadResult {
    FileName: string;
    FullWebPath: string;
    RuntimeInfo: string;
    EntityState: string;
}

once("SF-fileLine", () =>
    $.fn.fileLine = function (opt: FileLineOptions) {
        var fl = new FileLine(this, opt);
    });

export class FileLine extends Lines.EntityBase {
    options: FileLineOptions;
    constructor(element: JQuery, _options: FileLineOptions) {
        super(element, _options);
    }

    _create() {
        if (this.options.dragAndDrop == null || this.options.dragAndDrop == true)
            FileLine.initDragDrop(this.prefix.child("DivNew").get(),
                e=> this.fileDropped(e));

        this.prefix.child("sfFile").get().on("change", ev=> this.onChanged(ev)); 
    }

    static initDragDrop($div: JQuery, onDropped: (e: DragEvent) => void) {
        if (window.File && window.FileList && window.FileReader && new XMLHttpRequest().upload) {
            var self = this;
            var $fileDrop = $("<div></div>").addClass("sf-file-drop").html("drag a file here")
                .on("dragover", function (e) { FileLine.fileDropHover(e, true); })
                .on("dragleave", function (e) { FileLine.fileDropHover(e, false); })
                .appendTo($div);
            $fileDrop[0].addEventListener("drop", function (e) {
                FileLine.fileDropHover(e, false);
                onDropped(e);
            }, false);
        }
    }

    static fileDropHover(e, toggle: boolean) {
        e.stopPropagation();
        e.preventDefault();
        $(e.target).toggleClass("sf-file-drop-over", toggle);
    }

    uploadAsync(f: File, customizeXHR?: (xhr: XMLHttpRequest) => void) {
         this.prefix.child('loading').get().show();
        Entities.RuntimeInfo.setFromPrefix(this.options.prefix, new Entities.RuntimeInfo(this.singleType(), null, true));

        var fileName = f.name;

        var $divNew = this.prefix.child("DivNew").get();

        var xhr = new XMLHttpRequest();
        xhr.open("POST", this.options.uploadDroppedUrl || SF.Urls.uploadDroppedFile, true);
        xhr.setRequestHeader("X-FileName", fileName);
        xhr.setRequestHeader("X-Prefix", this.options.prefix);
        xhr.setRequestHeader("X-" + this.options.prefix.child(Entities.Keys.runtimeInfo), Entities.RuntimeInfo.getFromPrefix(this.options.prefix).toString());
        xhr.setRequestHeader("X-sfFileType", this.options.fileType);
        xhr.setRequestHeader("X-sfTabId", $("#sfTabId").val());

        var self = this;
        xhr.onload = function (e) {
            var result = <FileAsyncUploadResult>JSON.parse(xhr.responseText);

            self.onUploaded(result.FileName, result.FullWebPath, result.RuntimeInfo, result.EntityState);
        };

        xhr.onerror = function (e) {
            SF.log("Error " + xhr.statusText);
        };

        if (customizeXHR != null)
            customizeXHR(xhr);

        xhr.send(f);
    }

    fileDropped(e: DragEvent) {
        var files = e.dataTransfer.files;
        e.stopPropagation();
        e.preventDefault();

        this.uploadAsync(files[0]);
    }

    prepareSyncUpload() {
        //New file in FileLine but not to be uploaded asyncronously => prepare form for multipart and set runtimeInfo
        this.prefix.get()[0].setAttribute('value', (<HTMLInputElement>this.prefix.get()[0]).value);
        var $mform = $('form');
        $mform.attr('enctype', 'multipart/form-data').attr('encoding', 'multipart/form-data');
        Entities.RuntimeInfo.setFromPrefix(this.options.prefix, new Entities.RuntimeInfo(this.singleType(), null, true));
    }

    upload() {
        Entities.RuntimeInfo.setFromPrefix(this.options.prefix, new Entities.RuntimeInfo(this.singleType(), null, true));

        var $fileInput = this.prefix.get();
        $fileInput[0].setAttribute('value', (<HTMLInputElement>$fileInput[0]).value);
        this.prefix.child('loading').get().show();

        this.createTargetIframe();

        var url = this.options.uploadUrl || SF.Urls.uploadFile;

        var $fileForm = $('<form></form>')
            .attr('method', 'post').attr('enctype', 'multipart/form-data').attr('encoding', 'multipart/form-data')
            .attr('target', this.options.prefix.child("frame")).attr('action', url)
            .hide()
            .appendTo($('body'));

        var $divNew = this.prefix.child("DivNew").get();
        var $clonedDivNew = $divNew.clone(true);
        $divNew.after($clonedDivNew).appendTo($fileForm); //if not attached to our DOM first there are problems with filename

        $("<input type='hidden' name='" + this.options.prefix + "_sfFileType' value='" + this.options.fileType + "'/>").appendTo($fileForm);

        var $tabId = $("#" + Entities.Keys.tabId).clone().appendTo($fileForm);
        var $antiForgeryToken = $("input[name=" + Entities.Keys.antiForgeryToken + "]").clone().appendTo($fileForm);

        $fileForm.submit().remove();
    }

    createTargetIframe() {
        var name = this.options.prefix.child("frame");
        return $("<iframe id='" + name + "' name='" + name + "' src='about:blank' style='position:absolute;left:-1000px;top:-1000px'></iframe>")
            .appendTo($("body"));
    }

    setEntitySpecific(entityValue: Entities.EntityValue, itemPrefix?: string) {
        this.prefix.child(Entities.Keys.loading).get().hide();
        this.prefix.child("sfFile").get().val("");
        if (entityValue) {
            this.prefix.child(Entities.Keys.toStr).tryGet().html(entityValue.toStr);
            this.prefix.child(Entities.Keys.link).tryGet().html(entityValue.toStr).attr("href", entityValue.link);

            if (this.options.download == DownloadBehaviour.SaveAs)
                this.prefix.child(Entities.Keys.link).tryGet().attr("download", entityValue.toStr);

        } else {
            this.prefix.child(Entities.Keys.toStr).tryGet().html("");
            this.prefix.child(Entities.Keys.link).tryGet().html("").removeAttr("download").removeAttr("href");
        }
    }

    onUploaded(fileName: string, link: string, runtimeInfo: string, entityState: string) {

        this.setEntity(new Entities.EntityValue(Entities.RuntimeInfo.parse(runtimeInfo), fileName, link));

        this.prefix.child(Entities.Keys.entityState).tryGet().val(entityState);

        this.prefix.child("frame").tryGet().remove();
    }

    onChanged(e: Event) {
        if (this.options.asyncUpload) {
            this.upload();
        }
        else {
            this.prepareSyncUpload();
        }
    }

    updateButtonsDisplay() {
        var hasEntity = !!Entities.RuntimeInfo.getFromPrefix(this.options.prefix);

        
        this.prefix.child('DivOld').get().toggle(hasEntity);
        this.prefix.child('DivNew').get().toggle(!hasEntity);

        this.prefix.child("btnRemove").tryGet().toggle(hasEntity);
    }

    getLink(itemPrefix?: string): string {
        return this.prefix.child(Entities.Keys.link).get().attr("href") || null;
    }

    getToString(itemPrefix?: string): string {
        return this.prefix.child(Entities.Keys.link).get().text();
    }

}

