#BG:0
{ shuffle:
	- 	뭔가 떨어져 있네요. 일단 주워볼까요? #Name:친구 #Image:1 
	- 	저게 뭔가? 좋아 보이는걸?  #Name:도적 두목 #Image:2
	- 	일단 주워 두면 다 쓸모가 있겠지. #Name:주인공 #Image:0
}
-> get_stones

== get_stones ==
~ temp stone = RANDOM(1, 5)
마석을 {stone}개 얻었습니다. #Stone:{stone}
-> END