using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	public Transform b_sp;
	public Transform w_sp;
	public Transform c_sp;
	public Transform c_gr_sp;
	public Transform gr_sp;
	public Transform[] style1 = new Transform[41];
	public Transform[] style2_b = new Transform[8];
	public Transform[] style2_s = new Transform[8];
	public Transform[] style2_g = new Transform[25];
	private Transform[] style2 = new Transform[41];
	private int cur_style = -1;
	private int cur_n = 8;
	private GameObject current1;
	private GameObject current2;
	private Transform sp;
	private string extra_txt="";
	// Use this for initialization
	void Start () {
		style2_b.CopyTo(style2,0);
		style2_s.CopyTo(style2,8);
		style2_g.CopyTo(style2,16);
		//current1 = (Instantiate(style1[cur_n],b_sp.transform.position,b_sp.transform.rotation) as Transform).gameObject;
		Restart();
	}

	void OnGUI(){



		if (cur_style == -1){
			if (cur_n >=0 && cur_n<8)
				extra_txt = " (Big pixels)";
			if (cur_n >=8 && cur_n<16)
				extra_txt = " (Small pixels)";
			if (cur_n >=16)
				extra_txt = "";
			GUI.Label(new Rect(35, 10, 100, 30),"Style 1");
			if (GUI.Button(new Rect(290, 30, 90, 30), "Small Pixels"))
			{
				cur_n=8;
				Restart();
			}
			if (GUI.Button(new Rect(390, 30, 90, 30), "Big Pixels"))
			{
				cur_n=0;
				Restart();
			}
			if (GUI.Button(new Rect(490, 30, 90, 30), "Other effects"))
			{
				cur_n=16;
				Restart();
			}
		}
		if (cur_style == 1){
			GUI.Label(new Rect(35, 10, 100, 30),"Style 2");
			extra_txt = "";
		}
		GUI.Label(new Rect(10, 110, 300, 30),"Current Effect: " + current1.name.Substring(0, current1.name.Length - 7)+extra_txt);



		if (GUI.Button(new Rect(10, 30, 40, 30), "<-"))
		{
			cur_n=0;
			cur_style*=-1;
			Restart();
		}
		if (GUI.Button(new Rect(60, 30, 40, 30), "->"))
		{
			cur_n=0;
			cur_style*=-1;
			Restart();
		}
		if (GUI.Button(new Rect(110, 30, 100, 30), "Restart Effect"))
		{
			Restart();
		}
		if (GUI.Button(new Rect(10, 70, 100, 30), "Previous"))
		{
				cur_n--;
		
		
				if (cur_n > -1)
			{
				Restart();
			}
				else
				{
					cur_n = 40;
				Restart();
				}
			current1 = current2;

		}
			
		if (GUI.Button(new Rect(110, 70, 100, 30), "Next"))
		{
			cur_n++;
			print(current1);


			if (cur_n < 41){
				Restart();
			}
			else 
			{
				cur_n = 0;
				Restart();
			}
			print(current1);
			current1 = current2;
		}

	}
	
	// Update is called once per frame
	void Restart () {
		Destroy(current1);
		if (cur_style == 1){
			GameObject.Find("wall").GetComponent<Renderer>().enabled = false;
			if(style1[cur_n].name.Contains("horizontal") || style1[cur_n].name.Contains("Horizontal")){
				sp = w_sp;
				GameObject.Find("wall").GetComponent<Renderer>().enabled = true;
			}
			else if (style1[cur_n].name.Contains("Fireball") || style1[cur_n].name.Contains("Muzzle"))
				sp = c_sp;
			else if (style1[cur_n].name.Contains("Explosion") || style1[cur_n].name.Contains("torch")) 
				sp = gr_sp;
			else if (style1[cur_n].name.Contains("fountain")||style1[cur_n].name.Contains("Vertical")) 
				sp = c_gr_sp;
			else
				sp = b_sp;
			current2 = (Instantiate(style1[cur_n],sp.transform.position,sp.transform.rotation) as Transform).gameObject;
		}
		else {
			GameObject.Find("wall").GetComponent<Renderer>().enabled = false;
			if(style2[cur_n].name.Contains("horizontal") || style2[cur_n].name.Contains("Horizontal")){
				sp = w_sp;
				GameObject.Find("wall").GetComponent<Renderer>().enabled = true;
			}
			else if (style2[cur_n].name.Contains("Fireball") || style2[cur_n].name.Contains("Muzzle"))
				sp = c_sp;
			else if (style2[cur_n].name.Contains("Explosion")|| style2[cur_n].name.Contains("torch")) 
				sp = gr_sp;
			else if (style2[cur_n].name.Contains("fountain") ||style2[cur_n].name.Contains("Vertical")) 
				sp = c_gr_sp;
			else
				sp = b_sp;
			current2 = (Instantiate(style2[cur_n],sp.transform.position,sp.transform.rotation) as Transform).gameObject;
		}
		current1 = current2;
	}
}
