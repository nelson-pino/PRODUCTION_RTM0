!function(t,e,l){"use strict";var a=function(){a.modal=null,a.backdrop=null,a.opts=null,a.opts=a.extend({},a.defaults,a.getArguments(arguments)),a.open()},s=function(){a.modal=null,a.backdrop=null,a.opts=null,a.opts=a.extend({},a.defaults,a.getArguments(arguments)),a.open()};a.testMe=function(){console.log("I am OK!")},a.defaults={message:"",buttons:{},callbacks:{},tpl:{mbd:'<div id="modalBeta-backdrop" class="modal-backdrop fade"></div>',md:'<div id="modalBeta" class="modal m-alert fade " tabindex="-1" role="dialog"><div class="modal-dialog" role="document"><div class="modal-content">%hd% %bd% %ft%</div></div></div>',hd:"",bd:'<div class="modal-body">%message%</div>',ft:'<div class="modal-footer">%bs%</div>',bs:{ok:'<button type="button" class="btn btn-success">%label_ok%</button>',cancel:'<button type="button" class="btn btn-danger">%label_cancel%</button>',"default":'<button type="button" class="btn btn-default">%label_default%</button>'},lb:{ok:"Ok",cancel:"Cancel","default":"Ok"}},id:"modalBeta"},a.open=function(){a.bountyHunter(),a.create(),a.bindEvents(),a.show()},a.close=function(){a.modal.removeClass("in"),a.backdrop.removeClass("in"),setTimeout(function(){a.modal.parentNode.removeChild(a.modal),a.backdrop.parentNode.removeChild(a.backdrop),a.modal=a.backdrop=null,e.body.removeClass("modal-open")},500)},a.create=function(){var t=document.createElement("div");t.innerHTML=a.opts.tpl.mbd,e.body.insertBefore(t.firstChild,e.body.firstChild),a.backdrop=document.getElementById(a.opts.id+"-backdrop");var l="";if(0==Object.keys(Object(a.opts.buttons)).length)l=a.opts.tpl.bs["default"].template({"%label_default%":a.opts.tpl.lb["default"]});else for(var s in a.opts.buttons){var o={};o["%label_"+s+"%"]=a.opts.buttons[s].label,l+=a.opts.tpl.bs[s].template(o)}var n=document.createElement("div");n.innerHTML=a.opts.tpl.md.template({"%hd%":a.opts.tpl.hd,"%bd%":a.opts.tpl.bd.template({"%message%":a.opts.message}),"%ft%":a.opts.tpl.ft.template({"%bs%":l})}),e.body.insertBefore(n.firstChild,e.body.firstChild),a.modal=document.getElementById(a.opts.id)},a.bindEvents=function(){a.modal.getElementsByClassName("modal-footer")[0].addEventListener("click",function(t){var e=t.target;switch(!0){case e.hasClass("btn-default"):a.close(),"function"==typeof a.opts.callbacks["default"]&&a.opts.callbacks["default"].call();break;case e.hasClass("btn-success"):a.close(),"function"==typeof a.opts.callbacks.ok&&a.opts.callbacks.ok.call();break;case e.hasClass("btn-danger"):a.close(),"function"==typeof a.opts.callbacks.cancel&&a.opts.callbacks.cancel.call()}})},a.show=function(){e.body.classList.add("modal-open"),a.backdrop.addClass("in"),a.modal.style.display="block",a.modal.addClass("in")},a.bountyHunter=function(){},a.getArguments=function(t){var e={};switch(!0){case"object"==typeof t[0]:e=t[0];break;case"string"==typeof t[0]:switch(e.message=t[0],!0){case"string"==typeof t[1]||null==t[1]:""!=t[1]&&null!=t[1]&&(e.buttons={"default":{label:t[1]}}),"function"==typeof t[2]&&(e.callbacks={"default":t[2]});break;case"function"==typeof t[1]:e.buttons={ok:{label:"OK"}},e.callbacks={"default":t[1],ok:t[1]},"function"==typeof t[2]&&(e.buttons=a.extend(e.buttons,{cancel:{label:"Cancel"}}),e.callbacks=a.extend(e.callbacks,{cancel:t[2]}));break;case"object"==typeof t[1]:""!=t[1].label&&t[1].label!=l&&(e.buttons=a.extend(e.buttons||{},{ok:{label:t[1].label}})),""!=t[1].callback&&t[1].callback!=l&&"function"==typeof t[1].callback&&(e.callbacks=a.extend(e.callbacks||{},{"default":t[1].callback,ok:t[1].callback})),"string"==typeof t[2]?e.buttons=a.extend(e.buttons||{},{cancel:{label:t[2]}}):(""!=t[2].label&&t[2].label!=l&&(e.buttons=a.extend(e.buttons||{},{cancel:{label:t[2].label}})),""!=t[2].callback&&t[2].callback!=l&&"function"==typeof t[2].callback&&(e.callbacks=a.extend(e.callbacks||{},{cancel:t[2].callback})))}}return e},a.extend=function(){for(var t=arguments[0],e=1;e<arguments.length;e++)for(var l in arguments[e])arguments[e].hasOwnProperty(l)&&(t[l]=arguments[e][l]);return t},String.prototype.template=function(t){return 0==Object.keys(Object(t)).length?this:this.replace(/%\w+%/g,function(e){return e in t?t[e]:e})},Element.prototype.hasClass=function(t){return this.className.split(" ").indexOf(t)>=0},Element.prototype.addClass=function(t){this.hasClass(t)||(this.className+=" "+t)},Element.prototype.removeClass=function(t){this.className=this.className.replace(new RegExp("\\b"+t+"\\b"),"")},t.Alert=a,t.Confirm=s}(window,document);