	$('.initiativesnew').on('click',function()
	{
		var id = $(this).data('id');

		$.ajax({
        		url:url_setInitiative,
        		type:"POST",
        		data:{"_token":CSRF_TOKEN,'id' : id},
        		success:function(data)
        		{
					if(data.success == 1){
						window.location.href = url_getInitiative;
					}
				}
		});
		
	});


	$('.latestnews').on('click',function()
	{
		var id = $(this).data('lid');

		$.ajax({
        		url:url_setLatestNews,
        		type:"POST",
        		data:{"_token":CSRF_TOKEN,'id' : id},
        		success:function(data)
        		{
					if(data.success == 1){
						window.location.href = url_getLatestNews;
					}
				}
		});
		
	});


	
////////////////

$('.contactBtn').on('click',function(){
    var name 	= $('#name').val();
	var email 	= $("#email").val();
	var phone = $("#phone").val();
	var category  = $('#category').val();
	var message	 = $("#message").val();
	var recaptchaResponse = grecaptcha.getResponse();
	
    $.ajax({
			url:url_add_contact,
			type:"POST",
			data:{"_token":CSRF_TOKEN, "name":name, "email":email, "phone":phone, "category":category, "message":message, "g_recaptcha_response": recaptchaResponse},
			success:function(data)
			{
			   if(data.status == 0){
			        if(typeof data.msg.name !== 'undefined'){
						 $("#name-error").text(data.msg.name);  
                         $("#name").focus();  
					}if(typeof data.msg.email !== 'undefined'){
					    $("#email-error").text(data.msg.email);  
                        $("#email").focus();  
					}if(typeof data.msg.phone !== 'undefined'){
					    $("#phone-error").text(data.msg.phone);  
                        $("#phone").focus();  
					}if(typeof data.msg.category !== 'undefined'){
					    $("#category-error").text(data.msg.category);  
                        $("#category").focus();  
					}if(typeof data.msg.message !== 'undefined'){
					    $("#message-error").text(data.msg.message);  
                        $("#message").focus();  
					}
			   }else if(data.status == 1){
			      
			        $('#successModal').modal('show')
					setTimeout(function(){
					  window.location.reload();  
					},2000); 
			   }else if(data.status == 2){
					$("#message-error").text(data.msg);  
				}
			}
		});
});


$('.field').on('click', function() {
    var id = $(this).attr('id');
    $("#" + id + "-error").html('');
});


$('#email').on('click', function() {
    $("#emailsub-error").html('');
});

$('.pgmsearch').on('click',function(){
   var pgm = $('#program').val();
   window.location.href=url+'/programmes/'+pgm;
});


$(".searchInput").keyup(function (e) {
	 e.preventDefault();
     var search_query = $(this).val();
     if(search_query.length > 4){
    	 $.ajax({
    			url:url_searchData,
    			type:"POST",
    			dataType: "json",
    			data:{"_token":CSRF_TOKEN,'search_query' : search_query},
    			success:function(data)
    			{
    			    var html = link ='';
    			    $('.searchdata').html('');
    			   
                    if(data.success == 1){
                       for (var i = 0; i < data.searchData.length; i++) {
                          // link = url+"/"+ data.searchData[i].url;
                           html += "<li><p>" + data.searchData[i].menu_title +  "</p><a href="+ data.searchData[i].url +">"+  data.searchData[i].url +"</a></li>";
                       }
                       
                       if (html === '') {
        				  html = "No result found";
        				}
        				
        				$('.searchdata').html(html);
        				$('.results').html("Showing result "+data.searchData.length+" of "+ data.total);
        				
        				if( data.total > 1){
        				    
        				    $('.viewbtn').show();
        				    $(".viewbtn").attr("href", url+'/search-result/'+data.search_query);
        				}else{
        				     $('.viewbtn').hide();
        				}
                    }else{
                        $('.searchdata').html('No result found');
                    }
                     
    			}
    	});
     }else{
		  $('.searchdata').html('');
	 }
	 
 });
 
 
 
 /////////////////
 
 $(".subscribe").click(function (e) {
     
     var email = $('#email').val();
     $.ajax({
         	    url:url_subscribe,
    			type:"POST",
    			dataType: "json",
    			data:{"_token":CSRF_TOKEN,'email' : email},
    			success:function(data)
    			{
    			     if(data.status == 0){
    			        if(typeof data.msg.email !== 'undefined'){
    					    $("#emailsub-error").text(data.msg.email);  
    					    $("#emailsub-error").css({ "color":"#ff1212", "font-size":"15px"});
    					   
                            $("#email").focus();  
        				}
    			     }else if(data.status == 1){
    			         $("#emailsub-error").text('Subscription added');  
    					 $("#emailsub-error").css({ "color":"#34ec11", "font-size":"15px"});
    					 setTimeout(function(){
        					   $("#emailsub-error").text('');
        					   $("#email").val('');
        				  },2000); 
    			     }else if(data.status == 2){
    			         $("#emailsub-error").text(data.msg);  
    					 $("#emailsub-error").css({ "color":"#ff1212", "font-size":"15px"});
    			     }
    			}
     });
     
     
 });
 
 
 