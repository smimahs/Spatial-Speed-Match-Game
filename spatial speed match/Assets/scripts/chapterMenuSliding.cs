using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chapterMenuSliding : MonoBehaviour
{

	public static chapterMenuSliding refrence;

	public Vector2 startPos;
	public Vector2 endPos;
	private Camera camera;

	public enum MouseState
	{
		begin , moving , end , idle
	}
	public MouseState mouseState;

	public int currentChapterIndex;
	public int nextChapterIndex;
	
	public enum MoveDir
	{
		idle , left , right
	}
	public MoveDir moveDir;

	public Animator anim;

	public enum Page{page1,page2,page3}
	public Page page;
	
	
	private void Awake()
	{
		if (refrence == null)
		{
			refrence = this;
		}
		Application.targetFrameRate = 60;
		camera = Camera.main;
		mouseState = MouseState.idle;
		page = Page.page1;
		//country = CountryState.iran;
		moveDir = MoveDir.idle;
		currentChapterIndex = 1;
		nextChapterIndex = 1;
		anim = GetComponent<Animator>();
		
	}

	private void Start()
	{
		anim.SetTrigger("enter");
	}

	private void LateUpdate()
	{
		mouseController();
	}


	void mouseController()
	{
		
		#region begin

		if (Input.GetButtonDown("Fire1"))
		{
			if (mouseState == MouseState.idle)
			{
				
				mouseState = MouseState.begin;
				startPos = camera.ScreenToWorldPoint(Input.mousePosition);
			}
		}

		#endregion begin

		#region moving

		if (Input.GetButton("Fire1"))
		{
			if (mouseState == MouseState.begin)
			{
				mouseState = MouseState.moving;
			}

			if (mouseState == MouseState.moving)
			{
				endPos = camera.ScreenToWorldPoint(Input.mousePosition);
				if (Mathf.Abs(endPos.x - startPos.x) > .5f)
				{
					if (endPos.x > startPos.x)
					{
						moveDir = MoveDir.right;
					}
					else
					{
						moveDir = MoveDir.left;
					}
					changeChapter();
					mouseState = MouseState.idle;
				}
			}
			
		}

		#endregion moving

		#region ended

		if (Input.GetButtonUp("Fire1"))
		{
			if (mouseState == MouseState.moving)
			{
				mouseState = MouseState.idle;
			}
		}

		#endregion ended
		
	}


	public void changeChapter()
	{
		
		switch (moveDir)
		{
			case MoveDir.left:



				switch (page)
				{

					case Page.page1:

						page = Page.page2;
						anim.Play("p1p2");
						currentChapterIndex = 2;
						break;

					case Page.page2:
						page = Page.page3;
						anim.Play("p2p3");
						currentChapterIndex = 3;
						break;
				}


				break;
				
			case MoveDir.right:



				switch (page)
				{

					case Page.page2:

						page = Page.page1;
						anim.Play("p2p1");
						currentChapterIndex = 1;
						break;

					case Page.page3:
						page = Page.page2;
						anim.Play("p3p2");
						currentChapterIndex = 2;
						break;
				}


				break;
		}

	
	}
	
	
}
