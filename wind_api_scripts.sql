CREATE TABLE `point` (
	id int primary key,
    lat double not null,
    lng double not null
);

CREATE TABLE wind_day (
	id int primary key auto_increment,
	`date` date not null,
    point_id int not null,

    temperature double null,
    humidity double null,
    pressure double null,
    wind_bearing int null,
    wind_gust double null,
    wind_speed double null,
    
    constraint fk_wind_day_point foreign key (point_id) references `point`(id),
    index ix_wind_day_point_id (point_id)
);

CREATE TABLE wind_hour (
	id int primary key auto_increment,
	`date` datetime not null,
    wind_day_id int not null,

    temperature double null,
    humidity double null,
    pressure double null,
    wind_bearing int null,
    wind_gust double null,
    wind_speed double null,
    
    constraint fk_wind_hour_wind_day foreign key (wind_day_id) references `wind_day`(id),
    index ix_wind_hour_wind_day_id (wind_day_id)
);


CREATE TABLE date_api_call (
	id int primary key auto_increment,
    `date` datetime not null,
    `total` int not null
);

insert into `point` values
	(1,48.999160857142854,22.953650222222223),  
	(2,49.27394271428572,22.953650222222223), 
	(3,49.54872457142857,22.953650222222223), 
	(4,48.999160857142854,23.262819444444442),  
	(5,49.27394271428572,23.262819444444442), 
	(6,49.54872457142857,23.262819444444442), 
	(7,49.82350642857143,23.262819444444442), 
	(8,48.999160857142854,23.571988666666666),  
	(9,49.27394271428572,23.571988666666666), 
	(10,49.54872457142857,23.571988666666666), 
	(11,49.82350642857143,23.571988666666666), 
	(12,50.09828828571428,23.571988666666666), 
	(13,49.27394271428572,23.88115788888889),  
	(14,49.54872457142857,23.88115788888889),  
	(15,49.82350642857143,23.88115788888889),  
	(16,50.09828828571428,23.88115788888889),  
	(17,50.373070142857145,23.88115788888889), 
	(18,49.27394271428572,24.19032711111111),  
	(19,49.54872457142857,24.19032711111111),  
	(20,49.82350642857143,24.19032711111111),  
	(21,50.09828828571428,24.19032711111111),  
	(22,50.373070142857145,24.19032711111111), 
	(23,49.54872457142857,24.499496333333333), 
	(24,49.82350642857143,24.499496333333333), 
	(25,50.09828828571428,24.499496333333333), 
	(26,50.373070142857145,24.499496333333333),  
	(27,49.82350642857143,24.808665555555557), 
	(28,50.09828828571428,24.808665555555557), 
	(29,50.09828828571428,25.117834777777777), 
	(30,49.524173999999995,22.644481), 
	(31,49.943168,25.427004),  
	(32,48.724379,23.540712),  
	(33,50.647852,24.117774999999998);





