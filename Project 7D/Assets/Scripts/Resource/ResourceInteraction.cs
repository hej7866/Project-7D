using UnityEngine;

public class ResourceInteraction : MonoBehaviour
{
    private Animator animator;
    private ResourceNode targetNode;

    private float _gatherTimer = 0f;
    private bool _isHolding = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (targetNode != null)
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (!_isHolding)
                {
                    _isHolding = true;
                    animator.SetBool("isChopping", true);
                    UIManager.Instance.ShowGatherUI();
                }

                _gatherTimer += Time.deltaTime;
                UIManager.Instance.SetGatherProgress(_gatherTimer / targetNode.Data.gatherTime);

                if (_gatherTimer >= targetNode.Data.gatherTime)
                {
                    OnGatherComplete();
                }
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                ResetGathering();
            }
        }
        else
        {
            ResetGathering();
        }
    }

    void OnGatherComplete()
    {
        animator.SetBool("isChopping", false);
        Destroy(targetNode.gameObject);
        PlayerInventory.Instance.AddResource(targetNode.Data.icon, targetNode.ResourceType, targetNode.Data.category, targetNode.Data.amount);
        targetNode = null;
        ResetGathering();
    }

    void ResetGathering()
    {
        _isHolding = false;
        animator.SetBool("isChopping", false);
        _gatherTimer = 0f;
        UIManager.Instance.HideGatherUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ResourceNode node))
        {
            targetNode = node;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ResourceNode node) && node == targetNode)
        {
            targetNode = null;
            ResetGathering();
        }
    }
}
