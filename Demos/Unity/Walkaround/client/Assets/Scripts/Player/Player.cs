using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public static Vector2 DIR_UP = new Vector2(0, 1);
	public static Vector2 DIR_LEFT = new Vector2(-1, 0);
	public static Vector2 DIR_RIGHT = new Vector2(1, 0);
	public static Vector2 DIR_DOWN = new Vector2(0, -1);

	public Sprite StandDown;
	public Sprite StandLeft;
	public Sprite StandRight;
	public Sprite StandUp;
	public Sprite[] WalkDown;
	public Sprite[] WalkLeft;
	public Sprite[] WalkRight;
	public Sprite[] WalkUp;

	public float FramesPerSecond;

	public float Speed;
	public float ShotDelay;

	public PlayerStatus Status { get; private set; }
	public PlayerDirection Direction { get; private set; }
	public Vector2 Position { get; set; }
	public Vector3 Target { get; private set; }

	private SpriteRenderer _renderer;

	public bool CanShoot { get; private set; }
	public bool CanMove { get { return Status == PlayerStatus.Standing; } } 

	void Awake()
	{
		_renderer = renderer as SpriteRenderer;

		Position = transform.position;

		CanShoot = true;
	}
	
	void Update()
	{
		if (Status == PlayerStatus.Walking)
		{
			transform.position = Vector3.MoveTowards(transform.position, Target, Time.deltaTime * Speed);

			Sprite[] arr = null;

			// get the array of sprites for whatever direction the player is facing
			switch (Direction)
			{
				case PlayerDirection.Down:
					arr = WalkDown;
					break;

				case PlayerDirection.Left:
					arr = WalkLeft;
	                break;

				case PlayerDirection.Right:
					arr = WalkRight;
	                break;

				case PlayerDirection.Up:
					arr = WalkUp;
	                break;
            }

			// loop through the frames
			int index = (int)(Time.timeSinceLevelLoad * FramesPerSecond);
			index = index % arr.Length;
			_renderer.sprite = arr[index];

			// once the player gets to the target make them idle
			if (transform.position == Target)
			{
				if (!Input.anyKey)
					Face(Direction);

				Status = PlayerStatus.Standing;

				Vector2 pos = new Vector2((int)transform.position.x, (int)transform.position.y);
				Position = pos;
            }
        }
    }
    
	/// <summary>
	/// Make a player face a direction.
	/// </summary>
	/// <param name="direction">Direction to face.</param>
	public void Face(PlayerDirection direction)
	{
		switch (direction)
		{
			case PlayerDirection.Down:
				_renderer.sprite = StandDown;
				break;
				
			case PlayerDirection.Left:
				_renderer.sprite = StandLeft;
				break;
				
			case PlayerDirection.Right:
				_renderer.sprite = StandRight;
				break;
				
			case PlayerDirection.Up:
				_renderer.sprite = StandUp;
	            break;
        }
    }
    
	/// <summary>
	/// Make a player move to a position.  For making other players move in response to server events.
	/// </summary>
	/// <param name="position">Target position.</param>
	public void MoveTo(Vector2 position)
	{
		Vector2 current = new Vector2(transform.position.x, transform.position.y);
		if (position == current)
			return;

		Vector2 temp = (position - current).normalized;

		if (temp == DIR_UP)
			Direction = PlayerDirection.Up;
		else if (temp == DIR_LEFT)
			Direction = PlayerDirection.Left;
		else if (temp == DIR_RIGHT)
			Direction = PlayerDirection.Right;
		else if (temp == DIR_DOWN)
			Direction = PlayerDirection.Down;

		Status = PlayerStatus.Walking;
		Target = position;
	}

	/// <summary>
	/// Set a direction for a player to move in.
	/// </summary>
	/// <param name="direction">Target direction.</param>
    public void MoveTo(PlayerDirection direction)
    {
        if (Status == PlayerStatus.Walking)
            return;
        
        Direction = direction;
		Status = PlayerStatus.Walking;

		switch (direction)
		{
			case PlayerDirection.Down:
				Target = Position + DIR_DOWN;
				break;

			case PlayerDirection.Left:
				Target = Position + DIR_LEFT;
                break;

			case PlayerDirection.Right:
				Target = Position + DIR_RIGHT;
                break;

			case PlayerDirection.Up:
				Target = Position + DIR_UP;
                break;
        }
	}

	/// <summary>
	/// Get normalized vector for the direction the player is facing.
	/// </summary>
	/// <returns>The direction vector.</returns>
	public Vector2 GetDirectionVector()
	{
		switch (Direction)
		{
			case PlayerDirection.Down:
				return DIR_DOWN;
					
			case PlayerDirection.Left:
				return DIR_LEFT;
				
			case PlayerDirection.Right:
				return DIR_RIGHT;
				
			case PlayerDirection.Up:
				return DIR_UP;

			default:
				return Vector2.zero;
		}
	}

	/// <summary>
	/// Shoot a fireball.
	/// </summary>
	public void Shoot()
	{
		if (CanShoot)
		{
			Shot.Create(this, GetDirectionVector());
			StartCoroutine(ShotTimer());
			CanShoot = false;
		}
	}

	private IEnumerator ShotTimer()
	{
		yield return new WaitForSeconds(ShotDelay);
		
		CanShoot = true;
    }
}
