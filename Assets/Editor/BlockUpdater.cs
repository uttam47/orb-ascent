using AnalyticalApproach.OrbAscent.Editor;
using System;
using UnityEditor;
using UnityEngine;

struct HitData
{
    public bool upHit, downHit, leftHit, rightHit, forwardHit, backHit;


    public bool AllSidesHit()
    {
        return forwardHit && backHit && leftHit && rightHit;
    }

    public bool IsMiddleCase()
    {
        return (forwardHit && backHit) || (leftHit && rightHit);
    }
    public bool IsCorenerCase()
    {
        return ((leftHit && forwardHit) || (leftHit && backHit) || (rightHit && backHit) || (rightHit && forwardHit));
    }

    public bool NothingHit()
    {
        return !(upHit || leftHit || rightHit || forwardHit || backHit);
    }

    public bool ThreeSidesHit()
    {
        return leftHit && rightHit && forwardHit ||
        leftHit && rightHit && backHit ||
        backHit && forwardHit && leftHit ||
        backHit && forwardHit && rightHit;

    }
}
public class BlockUpdater : MonoBehaviour
{
    
    [MenuItem("OrbAscent/Remove Grass Blocks")]
    private static void ResetBuildingBlocks()
    {
        BlockRegistery blockRegistery = Resources.Load<BlockRegistery>(nameof(BlockRegistery));

        Transform parentTransform = Selection.activeGameObject.transform;

        if (!parentTransform.gameObject.CompareTag("BlocksContainer"))
        {
            Debug.Log(" Not running block updater on a blocksContainer"); 
            return;
        }
        
        BoxCollider[] children = parentTransform.GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider child in children)
        {
            if (child == parentTransform) continue; // Skip the parent object itself
            Vector3 position = child.transform.position;
            GameObject g = (GameObject)PrefabUtility.InstantiatePrefab(blockRegistery.GetBlock(BlockType.Plain), parentTransform);
            g.transform.position = position;
            DestroyImmediate(child.gameObject);
        }

    }


    [MenuItem("OrbAscent/Add Grass Blocks")]
    private static void UpdateBuildingBlock()
    {

        BlockRegistery blockRegistery = Resources.Load<BlockRegistery>(nameof(BlockRegistery));
        Transform parentTransform = Selection.activeGameObject.transform;

        if (!parentTransform.gameObject.CompareTag("BlocksContainer"))
        {
            Debug.Log(" Not running block updater on a blocksContainer"); 
            return;
        }

        Transform[] children = parentTransform.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child == parentTransform.transform) continue; // Skip the parent object itself

            // Structure to store hit data


            HitData hitData = new HitData();
            float hidDistance = 1;
            int layerMask = LayerMask.GetMask("BuildingBlock");

            // Cast rays in six directions
            hitData.upHit = Physics.Raycast(child.position, child.up, hidDistance, layerMask);
            hitData.downHit = Physics.Raycast(child.position, -child.up, hidDistance, layerMask);
            hitData.leftHit = Physics.Raycast(child.position, -child.right, hidDistance, layerMask);
            hitData.rightHit = Physics.Raycast(child.position, child.right, hidDistance, layerMask);
            hitData.forwardHit = Physics.Raycast(child.position, child.forward, hidDistance, layerMask);
            hitData.backHit = Physics.Raycast(child.position, -child.forward, hidDistance, layerMask);

            // Store the current position of the child
            Vector3 position = child.position;
            GameObject g = null;

            // Determine the correct building block to instantiate based on the hit data
            if (hitData.upHit || hitData.AllSidesHit())
            {
                g = (GameObject)PrefabUtility.InstantiatePrefab(blockRegistery.GetBlock(BlockType.Plain), parentTransform);
            }
            else if (hitData.NothingHit())
            {
                g = (GameObject)PrefabUtility.InstantiatePrefab(blockRegistery.GetBlock(BlockType.Isolated

                    ), parentTransform);
            }
            else if (hitData.ThreeSidesHit())
            {
                Vector3 direction = Vector3.zero;
                if (!hitData.leftHit) direction = -child.right;
                else if (!hitData.rightHit) direction = child.right;
                else if (!hitData.forwardHit) direction = child.forward;
                else if (!hitData.backHit) direction = -child.forward;
                g = (GameObject)PrefabUtility.InstantiatePrefab(blockRegistery.GetBlock(BlockType.Edge), parentTransform);
                g.transform.forward = direction;
            }
            else if (hitData.IsMiddleCase())
            {
                Vector3 direction = hitData.leftHit ? child.right : child.forward;
                g = (GameObject)PrefabUtility.InstantiatePrefab(blockRegistery.GetBlock(BlockType.Middle), parentTransform);
                g.transform.forward = direction;
            }
            else if (hitData.IsCorenerCase())
            {
                Vector3 direction = Vector3.zero;
                g = (GameObject)PrefabUtility.InstantiatePrefab(blockRegistery.GetBlock(BlockType.Corner), parentTransform);
                if (hitData.leftHit && hitData.forwardHit)
                {
                    direction = child.forward;
                }
                else if (hitData.leftHit && hitData.backHit)
                {
                    direction = -child.right;
                }
                else if (hitData.rightHit && hitData.backHit)
                {
                    direction = -child.forward;
                }
                else if (hitData.rightHit && hitData.forwardHit)
                {
                    direction = child.right;
                }
                g.transform.forward = direction;
            }
            else if (hitData.leftHit || hitData.forwardHit || hitData.rightHit || hitData.backHit)
            {
                Vector3 direction = Vector3.zero;

                if (hitData.leftHit) direction = -child.right;
                else if (hitData.rightHit) direction = child.right;
                else if (hitData.forwardHit) direction = child.forward;
                else if (hitData.backHit) direction = -child.forward;

                g = (GameObject)PrefabUtility.InstantiatePrefab(blockRegistery.GetBlock(BlockType.End), parentTransform);

                g.transform.forward = direction;
            }

            g.transform.position = position;
        }

        foreach (Transform child in children)
        {
            if (child == parentTransform) continue; // Skip the parent object itself
            DestroyImmediate(child.gameObject);
        }
    }
}