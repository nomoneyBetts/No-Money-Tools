using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoMoney.InputSystem
{
	public class InputReceiver : MonoBehaviour
	{
		[Header("Processor must implement IProcessor")]
		[Tooltip("Implement IProcessor")]
		[SerializeField]
		private MonoBehaviour _processor;
		private IProcessor _p;
        public IProcessor Processor
		{
			get
			{
				_p ??= _processor.GetComponent<IProcessor>();
				return _p;
			}
		}
		[SerializeField]
		private float _holdTime = 1f;

		[Header("Each Broadcaster must implement IBroadcaster")]
		[Tooltip("Implement IBroadcaster")]
		[SerializeField]
		private List<MonoBehaviour> _broadcasters;

		private InputData[]  _rawInputs;
		private List<InputData> _inputs;
        private readonly List<InputData> _heldInputs = new();

		private void Awake()
		{
			_rawInputs = new InputData[_broadcasters.Count];
			_inputs = new List<InputData>(_broadcasters.Count);
			for(int i = 0; i < _rawInputs.Length; i++)
			{
                MonoBehaviour element = _broadcasters[i];
                if (element == null)
				{
					Debug.LogWarning($"Broadcaster at index {i} is null");
					continue;
				}
				try
				{ 
					IBroadcaster broadcaster = (IBroadcaster)element;
                    broadcaster.OnBroadcast += input => _rawInputs[_broadcasters.IndexOf(element)] = input;
				}catch(InvalidCastException)
				{
					Debug.LogError($"Broadcaster at index {i} does not implement IBroadcaster");
				}
			}
		}

		private void LateUpdate()
		{
			// Search for collisions, if there are any, take the one with the most priority.
			_inputs = new(_rawInputs.Length);
			for(int i = 0; i < _rawInputs.Length; i++)
			{
				if (_rawInputs[i] == null) continue;

				InputData collision;
				if((collision = _inputs.Find(input => input.Name == _rawInputs[i].Name)) != null)
				{
					_inputs.Remove(collision);
					_inputs.Add(collision.Broadcaster.HandleCollision(collision, _rawInputs[i]));
                }
				else
				{
					_inputs.Add(_rawInputs[i]);
				}

				// Clean the array as we go
				_rawInputs[i] = null;
			}

			Processor.Process(_inputs);

			// Update held inputs. Remove any if expired.
			List<InputData> remove = new();
			foreach(InputData data in _heldInputs)
			{
				float delta = Time.realtimeSinceStartup - data.Timestamp;
				if(delta > _holdTime)
				{
					remove.Add(data);
				}
			}
			_heldInputs.RemoveAll(i => remove.Contains(i));
		}
	
		/// <summary>
		/// Some inputs come in pairs and don't arrive on the same frame (e.g. a touch and a location).
		/// Use this function to try to match inputs. If a match isn't found, the input will be held until
		/// a frame where the match is found, or the held number of frames expires.
		/// </summary>
		/// <param name="inputs">A list of inputs to process. Any input here is subject to being held.</param>
		/// <param name="filter">Return true if InputData matches the string.</param>
		/// <param name="action">Action to perform on the matching pair of inputs.</param>
		public void ProcessHeldInputs(List<InputData> inputs, Func<InputData, InputData, bool> filter, Action<InputData, InputData> action)
		{
			foreach (InputData d in inputs)
			{
				InputData match = _heldInputs.Find(m => filter(m, d));
				if (match == null)
				{
					_heldInputs.RemoveAll(i => i.Name == d.Name);
					_heldInputs.Add(d);
					continue;
				}
				_heldInputs.Remove(match);
				action(d, match);
			}
		}
	}
}
