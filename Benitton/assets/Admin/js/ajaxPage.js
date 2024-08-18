var xobj;
   //modern browers
   if(window.XMLHttpRequest)  {
	  xobj=new XMLHttpRequest();
	  }
	  //for ie
	  else if(window.ActiveXObject)   {
	    xobj=new ActiveXObject("Microsoft.XMLHTTP");
		}
		else {
		  alert("Your broweser doesnot support ajax");
		  }	
      // >>>>>>>>>>>>>>>>>>>>  START WEBSITE OPERATION DIR  >>>>>>>>>>>>>>>>>>>>>>>>        
	  
// create by qc searching buyer item code for create po >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

		 function fetch_machine(catid)
		{	
			if(xobj)
			{
			    
			    var pre_addmoremachineids = document.getElementById("addmoremachineids").value;
			    //alert(pre_addmoremachineids);
				xobj.open("GET","fetch_machine.php?catid="+catid+"&mchid="+pre_addmoremachineids);			
				xobj.onreadystatechange=function() 
				{
						if(xobj.readyState==4 && xobj.status==200) 
					{
										   //alert(xobj.responseText);
                     document.getElementById('showadditem').innerHTML= xobj.responseText;
                     
                     // start checked selected machine while select machine category on add quotation
                     //var pre_addmoremachineids = document.getElementById("addmoremachineids").value;
                     //alert(pre_addmoremachineids);
                     
                    arrmachineid = pre_addmoremachineids.split(', ');
                    for(i=0; i < arrmachineid.length; i++){
                        idd = 'checkUnchackMachine'+arrmachineid[i];
                        document.getElementById(idd).checked=true;
                    }
                     // close checked selected machine while select machine category on add quotation
                     
					} 
				}                         
					  xobj.send(null);
				}
				}
				
				// add more machine in qutation
				
	    function add_machine(machineid,machineName)
		{
		
			       var pre_machinenames = document.getElementById("showaddMachine").innerHTML;
			       var pre_addmoremachineids = document.getElementById("addmoremachineids").value;
			      //alert(pre_machinenames);
			    //  alert(pre_addmoremachineids);
                // machineName = "<button class='btn btn-success' id='"+machineid+"'>"+machineName+" <i class='fa fa-trash'></i> </button>";
			       
			        machineName = "<b>"+machineName+"</b>";
			      // alert(machineName);
			      if(pre_addmoremachineids == ""){
			        document.getElementById("showaddMachine").innerHTML = machineName+', ';
			        document.getElementById("addmoremachineids").value = machineid+', ';
			      }else
			        {
			            //checkStatus = document.getElementById("checkUnchackMachine").checked;
    			           if(pre_addmoremachineids.includes(machineid) )
    			           {
    			               //alert(machineName);
    			               //alert(pre_machinenames);
    			                //document.getElementById(machineid).style.display = "none";
    			                
                           remove_mname = pre_machinenames.replace(machineName+',', "");
                           remove_ids = pre_addmoremachineids.replace(machineid+',', "");
                           
                           document.getElementById("showaddMachine").innerHTML = remove_mname;
    			           document.getElementById("addmoremachineids").value = remove_ids;
                            }else
                            {
              
                           addmoremname = pre_machinenames+machineName+', ';
    			           addmoreids = pre_addmoremachineids+machineid+', ';
    			           document.getElementById("showaddMachine").innerHTML = addmoremname;
    			           document.getElementById("addmoremachineids").value = addmoreids;
                          }
			          
			      
			       }
 

   
   
		
				}
		
		function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
		  
		  
		  
		  