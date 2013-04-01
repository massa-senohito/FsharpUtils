namespace Lib
  module Animate=
  //座標計算などはhttps://github.com/gyuque/sw3d/blob/master/sw3d/core/geometry-core.js

    ///多分何か汎かできると思うんだ、required(/)が邪魔
    /// <summary>気持ちいい感じのアニメーション</summary>
    /// <param name="delta">大きいほどゆっくり変化</param>
    let inline Curve current dist delta=
      (dist-current)/delta
    ///<summary>マリオジャンプ、最初のタプルはいるべきy座標、次のタプルは前のy座標</summary>
    ///<param name="pos">現在の座標</param>
    ///<param name="prev">前の座標</param>
    ///<param name="force">ジャンプの強さ</param>
    let inline marioJump pos prev force=
      let temp=pos
      pos+pos-prev+force,temp

    

    let rec it cur pre=
      match marioJump cur pre -1. with
      |0.,_->[]
      |c,p->Util.pf c; it c p
    let tes<'a>=it 10. 0. //(10,0),空中ではforce=-1=


//damping ≧ 2 （stiffness × mass) ^ (1 / 2)
//public Vector3 Moveto(Vector3 nowPosition, Vector3 target)
//	{
//		stretch=nowPosition-target;
//		force= -stiffness*stretch - damping * velocity;
//		acceleration=force/mass;
//		velocity += acceleration * Time.deltaTime;
//		nowPosition+= velocity * Time.deltaTime;
//		return nowPosition;
//	}
    type SpringMove(stiffness,damping,mass)=
      let stiffness=stiffness
      let damping=damping
      let mass=mass
      member  this.force s v= -stiffness * s - damping *v
      member inline this.stretch pos target=pos-target
      member inline this.moveTo curpos target=
        let st=this.stretch curpos target
        st 
    module MathUtil=
      let agezokornd (list:('a*bool) seq) coinrate=
        fun (coin:int)->
          Seq.skip (coinrate coin)  list 
      //0-アイテム数分の乱数、手に入れていないものを下にソート、資金を増やせば最小値があがり、手に入れていないアイテムを手に入れやすくする
    module GameL4Cs=

      type Controller (chara,position)=
        let mutable oldpos=0
        let mutable pos=position
        member this.chara=chara
        ///宙にいたらforce=-1
        member this.Jump force=
          let t=marioJump pos oldpos force
          pos<-t |>fst;oldpos<-t |> snd
